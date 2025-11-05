# Deployment & Infrastructure

## **Overview**

This section covers containerization, orchestration, CI/CD pipelines, and infrastructure requirements for deploying the AI Interview Preparation System across different environments with focus on scalability and WebRTC capabilities.

## **Containerization**

### **API Dockerfile**
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/AbpResearchWorker.HttpApi.Host/AbpResearchWorker.HttpApi.Host.csproj", "src/AbpResearchWorker.HttpApi.Host/"]
COPY ["src/AbpResearchWorker.Application/AbpResearchWorker.Application.csproj", "src/AbpResearchWorker.Application/"]
COPY ["src/AbpResearchWorker.Domain/AbpResearchWorker.Domain.csproj", "src/AbpResearchWorker.Domain/"]
COPY ["src/AbpResearchWorker.EntityFrameworkCore/AbpResearchWorker.EntityFrameworkCore.csproj", "src/AbpResearchWorker.EntityFrameworkCore/"]
COPY ["src/AbpResearchWorker.HttpApi/AbpResearchWorker.HttpApi.csproj", "src/AbpResearchWorker.HttpApi/"]

# Restore dependencies
RUN dotnet restore "src/AbpResearchWorker.HttpApi.Host/AbpResearchWorker.HttpApi.Host.csproj"

# Copy source code
COPY . .

# Build application
WORKDIR "/src/src/AbpResearchWorker.HttpApi.Host"
RUN dotnet build "AbpResearchWorker.HttpApi.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AbpResearchWorker.HttpApi.Host.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install dependencies for WebRTC and health checks
RUN apt-get update && apt-get install -y \
    curl \
    ffmpeg \
    libssl3 \
    && rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published application
COPY --from=publish /app/publish .

# Create directories for WebRTC recordings and logs
RUN mkdir -p /app/recordings /app/logs

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

# Expose ports
EXPOSE 80 443 8080 8443

ENTRYPOINT ["dotnet", "AbpResearchWorker.HttpApi.Host.dll"]
```

### **Angular Dockerfile**
```dockerfile
# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files
COPY angular/package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy source code
COPY angular/ .

# Build for production
RUN npm run build:prod

# Runtime stage
FROM nginx:alpine AS final

# Install envsubst for dynamic configuration
RUN apk add --no-cache gettext

# Copy built application
COPY --from=build /app/dist/AbpResearchWorker /usr/share/nginx/html

# Copy nginx configuration
COPY angular/nginx.conf /etc/nginx/nginx.conf
COPY angular/default.conf.template /etc/nginx/templates/default.conf.template

# Create startup script for dynamic configuration
RUN echo '#!/bin/sh' > /docker-entrypoint.d/40-generate-config.sh && \
    echo 'envsubst < /etc/nginx/templates/default.conf.template > /etc/nginx/conf.d/default.conf' >> /docker-entrypoint.d/40-generate-config.sh && \
    chmod +x /docker-entrypoint.d/40-generate-config.sh

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

### **Docker Compose (Development)**
```yaml
version: '3.8'

services:
  interview-prep-api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
      - "8080:8080"   # WebRTC signaling
      - "8443:8443"   # WebRTC over HTTPS
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - OPENAI_API_KEY=${OPENAI_API_KEY}
      - DATABASE_CONNECTION_STRING=Server=database;Database=AbpResearchWorker_Dev;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=true
      - REDIS_CONNECTION_STRING=redis:6379,password=RedisPassword123
      - ABP_LICENSE_CODE=${ABP_LICENSE_CODE}
    volumes:
      - interview-recordings:/app/recordings
      - interview-logs:/app/logs
      - ~/.aspnet/https:/https:ro
    depends_on:
      - database
      - redis
      - seq
    networks:
      - interview-network
    restart: unless-stopped

  interview-prep-ui:
    build:
      context: .
      dockerfile: angular/Dockerfile
    ports:
      - "4200:80"
    environment:
      - API_BASE_URL=http://interview-prep-api:5000
      - WEBRTC_SIGNALING_URL=ws://interview-prep-api:8080
    depends_on:
      - interview-prep-api
    networks:
      - interview-network
    restart: unless-stopped

  database:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=YourStrong@Password123
      - ACCEPT_EULA=Y
      - MSSQL_PID=Developer
    ports:
      - "1433:1433"
    volumes:
      - mssql-data:/var/opt/mssql
    networks:
      - interview-network
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis-data:/data
    command: redis-server --appendonly yes --requirepass RedisPassword123
    networks:
      - interview-network
    restart: unless-stopped

  seq:
    image: datalust/seq:latest
    ports:
      - "5341:80"
    environment:
      - ACCEPT_EULA=Y
      - SEQ_FIRSTRUN_ADMINPASSWORDHASH=QH+cMEqLjhyJk6a/E0Txk5ANa4FR9vI7qj+2qSyVFfV11+Z+24sKrTs7WEjgisIFPE2LmjC3GW5Cl7QFYOqJzTBY4QXUf8Q5IfB7A+QMrOyeUdf4lKJ+EJ4Z4Z4Z4Z4=
    volumes:
      - seq-data:/data
    networks:
      - interview-network
    restart: unless-stopped

  # WebRTC TURN server for production-like testing
  coturn:
    image: coturn/coturn:latest
    ports:
      - "3478:3478/udp"
      - "3478:3478/tcp"
      - "49152-65535:49152-65535/udp"
    environment:
      - TURN_USERNAME=interview_user
      - TURN_PASSWORD=interview_pass
    command: [
      "-n",
      "--log-file=stdout",
      "--lt-cred-mech",
      "--fingerprint",
      "--no-multicast-peers",
      "--no-cli",
      "--no-tlsv1",
      "--no-tlsv1_1"
    ]
    networks:
      - interview-network
    restart: unless-stopped

volumes:
  mssql-data:
  redis-data:
  seq-data:
  interview-recordings:
  interview-logs:

networks:
  interview-network:
    driver: bridge
```

## **Kubernetes Deployment**

### **Namespace**
```yaml
# k8s/namespace.yaml
apiVersion: v1
kind: Namespace
metadata:
  name: interview-prep
  labels:
    name: interview-prep
```

### **ConfigMap**
```yaml
# k8s/configmap.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: interview-prep-config
  namespace: interview-prep
data:
  appsettings.Production.json: |
    {
      "App": {
        "SelfUrl": "https://api.interview-prep.company.com",
        "AngularUrl": "https://interview-prep.company.com"
      },
      "Interview": {
        "MaxConcurrentSessions": 20,
        "SessionTimeoutMinutes": 60
      },
      "WebRTC": {
        "Enabled": true,
        "IceServers": [
          {
            "Urls": ["stun:stun.l.google.com:19302"]
          },
          {
            "Urls": ["turn:turn.interview-prep.company.com:3478"],
            "Username": "interview_user",
            "Credential": "interview_pass"
          }
        ]
      },
      "RateLimiting": {
        "RequestsPerMinute": 60,
        "RequestsPerHour": 500,
        "CostLimitPerDay": 50.00,
        "SessionLimitPerUser": 15
      },
      "Monitoring": {
        "Enabled": true,
        "AlertThresholds": {
          "ErrorRatePercent": 2,
          "ResponseTimeMs": 5000,
          "ActiveSessionsLimit": 100
        }
      }
    }
  
  nginx.conf: |
    upstream api_backend {
      server interview-prep-api-service:80;
    }
    
    upstream websocket_backend {
      server interview-prep-api-service:8080;
    }
    
    server {
      listen 80;
      server_name _;
      
      # API requests
      location /api/ {
        proxy_pass http://api_backend;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
      }
      
      # WebRTC signaling
      location /ws/ {
        proxy_pass http://websocket_backend;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
      }
      
      # Static files
      location / {
        root /usr/share/nginx/html;
        try_files $uri $uri/ /index.html;
      }
      
      # Health check
      location /health {
        access_log off;
        return 200 "healthy\n";
      }
    }
```

### **Secrets**
```yaml
# k8s/secrets.yaml
apiVersion: v1
kind: Secret
metadata:
  name: interview-prep-secrets
  namespace: interview-prep
type: Opaque
data:
  openai-api-key: <base64-encoded-api-key>
  database-connection: <base64-encoded-connection-string>
  redis-connection: <base64-encoded-redis-connection>
  jwt-secret: <base64-encoded-jwt-secret>
  abp-license-code: <base64-encoded-abp-license>
  turn-server-credentials: <base64-encoded-turn-credentials>
```

### **API Deployment**
```yaml
# k8s/api-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: interview-prep-api
  namespace: interview-prep
  labels:
    app: interview-prep-api
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  selector:
    matchLabels:
      app: interview-prep-api
  template:
    metadata:
      labels:
        app: interview-prep-api
    spec:
      containers:
      - name: interview-prep-api
        image: interview-prep-api:latest
        ports:
        - containerPort: 80
          name: http
        - containerPort: 443
          name: https
        - containerPort: 8080
          name: websocket
        - containerPort: 8443
          name: websocket-ssl
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: OPENAI_API_KEY
          valueFrom:
            secretKeyRef:
              name: interview-prep-secrets
              key: openai-api-key
        - name: DATABASE_CONNECTION_STRING
          valueFrom:
            secretKeyRef:
              name: interview-prep-secrets
              key: database-connection
        - name: REDIS_CONNECTION_STRING
          valueFrom:
            secretKeyRef:
              name: interview-prep-secrets
              key: redis-connection
        - name: ABP_LICENSE_CODE
          valueFrom:
            secretKeyRef:
              name: interview-prep-secrets
              key: abp-license-code
        volumeMounts:
        - name: config-volume
          mountPath: /app/appsettings.Production.json
          subPath: appsettings.Production.json
        - name: recordings-storage
          mountPath: /app/recordings
        resources:
          requests:
            memory: "512Mi"
            cpu: "500m"
          limits:
            memory: "1Gi"
            cpu: "1000m"
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 60
          periodSeconds: 30
          timeoutSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 80
          initialDelaySeconds: 10
          periodSeconds: 10
          timeoutSeconds: 5
      volumes:
      - name: config-volume
        configMap:
          name: interview-prep-config
      - name: recordings-storage
        persistentVolumeClaim:
          claimName: recordings-pvc
      imagePullSecrets:
      - name: registry-secret
```

### **UI Deployment**
```yaml
# k8s/ui-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: interview-prep-ui
  namespace: interview-prep
  labels:
    app: interview-prep-ui
spec:
  replicas: 2
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  selector:
    matchLabels:
      app: interview-prep-ui
  template:
    metadata:
      labels:
        app: interview-prep-ui
    spec:
      containers:
      - name: interview-prep-ui
        image: interview-prep-ui:latest
        ports:
        - containerPort: 80
          name: http
        env:
        - name: API_BASE_URL
          value: "https://api.interview-prep.company.com"
        - name: WEBRTC_SIGNALING_URL
          value: "wss://api.interview-prep.company.com/ws"
        volumeMounts:
        - name: nginx-config
          mountPath: /etc/nginx/nginx.conf
          subPath: nginx.conf
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 30
        readinessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 10
      volumes:
      - name: nginx-config
        configMap:
          name: interview-prep-config
```

### **Services**
```yaml
# k8s/services.yaml
apiVersion: v1
kind: Service
metadata:
  name: interview-prep-api-service
  namespace: interview-prep
  labels:
    app: interview-prep-api
spec:
  selector:
    app: interview-prep-api
  ports:
  - name: http
    port: 80
    targetPort: 80
    protocol: TCP
  - name: https
    port: 443
    targetPort: 443
    protocol: TCP
  - name: websocket
    port: 8080
    targetPort: 8080
    protocol: TCP
  - name: websocket-ssl
    port: 8443
    targetPort: 8443
    protocol: TCP
  type: ClusterIP

---
apiVersion: v1
kind: Service
metadata:
  name: interview-prep-ui-service
  namespace: interview-prep
  labels:
    app: interview-prep-ui
spec:
  selector:
    app: interview-prep-ui
  ports:
  - name: http
    port: 80
    targetPort: 80
    protocol: TCP
  type: ClusterIP
```

### **Horizontal Pod Autoscaler**
```yaml
# k8s/hpa.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: interview-prep-api-hpa
  namespace: interview-prep
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: interview-prep-api
  minReplicas: 3
  maxReplicas: 15
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 60
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 70
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 60
      policies:
      - type: Percent
        value: 50
        periodSeconds: 15
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
      - type: Percent
        value: 10
        periodSeconds: 60

---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: interview-prep-ui-hpa
  namespace: interview-prep
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: interview-prep-ui
  minReplicas: 2
  maxReplicas: 8
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

### **Ingress**
```yaml
# k8s/ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: interview-prep-ingress
  namespace: interview-prep
  annotations:
    kubernetes.io/ingress.class: "nginx"
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
    nginx.ingress.kubernetes.io/rate-limit: "100"
    nginx.ingress.kubernetes.io/rate-limit-window: "1m"
    nginx.ingress.kubernetes.io/proxy-read-timeout: "300"
    nginx.ingress.kubernetes.io/proxy-send-timeout: "300"
    nginx.ingress.kubernetes.io/websocket-services: "interview-prep-api-service"
spec:
  tls:
  - hosts:
    - interview-prep.company.com
    - api.interview-prep.company.com
    secretName: interview-prep-tls
  rules:
  - host: api.interview-prep.company.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: interview-prep-api-service
            port:
              number: 80
  - host: interview-prep.company.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: interview-prep-ui-service
            port:
              number: 80
```

## **CI/CD Pipeline**

### **GitHub Actions Workflow**
```yaml
# .github/workflows/deploy.yml
name: Build and Deploy Interview Prep System

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

env:
  DOCKER_REGISTRY: ghcr.io
  API_IMAGE_NAME: interview-prep-api
  UI_IMAGE_NAME: interview-prep-ui

jobs:
  test:
    runs-on: ubuntu-latest
    services:
      redis:
        image: redis:7-alpine
        ports:
          - 6379:6379
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          SA_PASSWORD: TestPassword123!
          ACCEPT_EULA: Y
        ports:
          - 1433:1433
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
        
    - name: Setup Node.js
      uses: actions/setup-node@v4
      with:
        node-version: '20'
        
    - name: Restore .NET dependencies
      run: dotnet restore
      
    - name: Build .NET solution
      run: dotnet build --no-restore
      
    - name: Run .NET tests
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
      env:
        OPENAI_API_KEY: test-key
        DATABASE_CONNECTION_STRING: "Server=localhost;Database=AbpResearchWorker_Test;User Id=sa;Password=TestPassword123!;TrustServerCertificate=true"
        
    - name: Install Angular dependencies
      working-directory: ./angular
      run: npm ci
      
    - name: Run Angular tests
      working-directory: ./angular
      run: npm run test:ci
      
    - name: Run Angular lint
      working-directory: ./angular
      run: npm run lint
      
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3

  build-and-push:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main' || github.ref == 'refs/heads/develop'
    steps:
    - uses: actions/checkout@v4
    
    - name: Set up Docker Buildx
      uses: docker/setup-buildx-action@v3
      
    - name: Log in to Container Registry
      uses: docker/login-action@v3
      with:
        registry: ${{ env.DOCKER_REGISTRY }}
        username: ${{ github.actor }}
        password: ${{ secrets.GITHUB_TOKEN }}
        
    - name: Extract API metadata
      id: api-meta
      uses: docker/metadata-action@v5
      with:
        images: ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.API_IMAGE_NAME }}
        tags: |
          type=ref,event=branch
          type=ref,event=pr
          type=sha,prefix={{branch}}-
          
    - name: Extract UI metadata
      id: ui-meta
      uses: docker/metadata-action@v5
      with:
        images: ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.UI_IMAGE_NAME }}
        tags: |
          type=ref,event=branch
          type=ref,event=pr
          type=sha,prefix={{branch}}-
          
    - name: Build and push API image
      uses: docker/build-push-action@v5
      with:
        context: .
        file: ./Dockerfile
        push: true
        tags: ${{ steps.api-meta.outputs.tags }}
        labels: ${{ steps.api-meta.outputs.labels }}
        cache-from: type=gha
        cache-to: type=gha,mode=max
        
    - name: Build and push UI image
      uses: docker/build-push-action@v5
      with:
        context: .
        file: ./angular/Dockerfile
        push: true
        tags: ${{ steps.ui-meta.outputs.tags }}
        labels: ${{ steps.ui-meta.outputs.labels }}
        cache-from: type=gha
        cache-to: type=gha,mode=max

  deploy-staging:
    needs: build-and-push
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/develop'
    environment: staging
    steps:
    - uses: actions/checkout@v4
    
    - name: Deploy to Staging
      uses: azure/k8s-deploy@v1
      with:
        manifests: |
          k8s/namespace.yaml
          k8s/configmap.yaml
          k8s/secrets.yaml
          k8s/api-deployment.yaml
          k8s/ui-deployment.yaml
          k8s/services.yaml
          k8s/hpa.yaml
          k8s/ingress.yaml
        images: |
          ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.API_IMAGE_NAME }}:develop
          ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.UI_IMAGE_NAME }}:develop
        kubectl-version: 'latest'
        
    - name: Run E2E Tests
      run: |
        # Wait for deployment to be ready
        kubectl wait --for=condition=available --timeout=300s deployment/interview-prep-api -n interview-prep
        kubectl wait --for=condition=available --timeout=300s deployment/interview-prep-ui -n interview-prep
        # Run E2E tests against staging
        npm run e2e:staging

  deploy-production:
    needs: build-and-push
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    environment: production
    steps:
    - uses: actions/checkout@v4
    
    - name: Deploy to Production
      uses: azure/k8s-deploy@v1
      with:
        manifests: |
          k8s/namespace.yaml
          k8s/configmap.yaml
          k8s/secrets.yaml
          k8s/api-deployment.yaml
          k8s/ui-deployment.yaml
          k8s/services.yaml
          k8s/hpa.yaml
          k8s/ingress.yaml
        images: |
          ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.API_IMAGE_NAME }}:main
          ${{ env.DOCKER_REGISTRY }}/${{ github.repository_owner }}/${{ env.UI_IMAGE_NAME }}:main
        kubectl-version: 'latest'
        
    - name: Health Check
      run: |
        # Wait for deployment and verify health
        kubectl wait --for=condition=available --timeout=600s deployment/interview-prep-api -n interview-prep
        kubectl wait --for=condition=available --timeout=600s deployment/interview-prep-ui -n interview-prep
        
        # Verify endpoints
        curl -f https://api.interview-prep.company.com/health
        curl -f https://interview-prep.company.com/health
```

## **Monitoring & Observability**

### **Prometheus Configuration**
```yaml
# monitoring/prometheus.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: prometheus-config
data:
  prometheus.yml: |
    global:
      scrape_interval: 15s
    scrape_configs:
    - job_name: 'interview-prep-api'
      static_configs:
      - targets: ['interview-prep-api-service:80']
      metrics_path: '/metrics'
      scrape_interval: 10s
    - job_name: 'interview-prep-ui'
      static_configs:
      - targets: ['interview-prep-ui-service:80']
      metrics_path: '/metrics'
      scrape_interval: 30s
```

### **Grafana Dashboard Config**
```json
{
  "dashboard": {
    "title": "AI Interview Preparation System",
    "panels": [
      {
        "title": "Active Interview Sessions",
        "type": "singlestat",
        "targets": [
          {
            "expr": "sum(active_interview_sessions)",
            "legendFormat": "Active Sessions"
          }
        ]
      },
      {
        "title": "Interview Success Rate",
        "type": "graph",
        "targets": [
          {
            "expr": "rate(interview_sessions_completed_total[5m]) / rate(interview_sessions_started_total[5m]) * 100",
            "legendFormat": "Success Rate %"
          }
        ]
      },
      {
        "title": "AI Cost per Hour",
        "type": "graph", 
        "targets": [
          {
            "expr": "sum(increase(openai_cost_total[1h]))",
            "legendFormat": "Hourly Cost ($)"
          }
        ]
      },
      {
        "title": "WebRTC Connection Quality",
        "type": "graph",
        "targets": [
          {
            "expr": "avg(webrtc_latency_ms)",
            "legendFormat": "Average Latency (ms)"
          },
          {
            "expr": "avg(webrtc_packet_loss_rate) * 100",
            "legendFormat": "Packet Loss Rate (%)"
          }
        ]
      }
    ]
  }
}
```

## **Infrastructure as Code**

### **Terraform Configuration**
```hcl
# infrastructure/main.tf
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "interview_prep" {
  name     = "rg-interview-prep-${var.environment}"
  location = var.location
}

resource "azurerm_kubernetes_cluster" "interview_prep" {
  name                = "aks-interview-prep-${var.environment}"
  location            = azurerm_resource_group.interview_prep.location
  resource_group_name = azurerm_resource_group.interview_prep.name
  dns_prefix          = "aks-interview-prep-${var.environment}"

  default_node_pool {
    name                = "default"
    node_count          = 3
    vm_size             = "Standard_D4s_v3"  # More CPU/RAM for WebRTC
    enable_auto_scaling = true
    min_count          = 3
    max_count          = 10
  }

  identity {
    type = "SystemAssigned"
  }

  network_profile {
    network_plugin = "azure"
  }
}

resource "azurerm_mssql_server" "interview_prep" {
  name                         = "sql-interview-prep-${var.environment}"
  resource_group_name          = azurerm_resource_group.interview_prep.name
  location                     = azurerm_resource_group.interview_prep.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_username
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_mssql_database" "interview_prep" {
  name      = "interview-prep-db"
  server_id = azurerm_mssql_server.interview_prep.id
  sku_name  = "S2"  # Standard tier for production
}

resource "azurerm_redis_cache" "interview_prep" {
  name                = "redis-interview-prep-${var.environment}"
  location            = azurerm_resource_group.interview_prep.location
  resource_group_name = azurerm_resource_group.interview_prep.name
  capacity            = 2
  family              = "C"
  sku_name            = "Standard"
  enable_non_ssl_port = false
}

resource "azurerm_storage_account" "interview_recordings" {
  name                     = "stintviewrec${var.environment}"
  resource_group_name      = azurerm_resource_group.interview_prep.name
  location                 = azurerm_resource_group.interview_prep.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "recordings" {
  name                  = "recordings"
  storage_account_name  = azurerm_storage_account.interview_recordings.name
  container_access_type = "private"
}
```

## **Deployment Checklist**

### **Pre-deployment:**
- [ ] All tests passing (unit, integration, E2E, load)
- [ ] Security scan completed (OWASP, dependency check)
- [ ] ABP license configured for target environment
- [ ] Database migrations prepared and tested
- [ ] OpenAI API keys and quotas configured
- [ ] WebRTC TURN server configured and tested
- [ ] Secrets configured in target environment
- [ ] Health check endpoints verified
- [ ] Monitoring dashboards configured
- [ ] SSL certificates provisioned

### **Deployment:**
- [ ] Blue-green deployment strategy configured
- [ ] Rolling update parameters set (maxSurge, maxUnavailable)
- [ ] Database migrations applied successfully
- [ ] Redis cache warmed with initial data
- [ ] WebRTC infrastructure validated
- [ ] Load balancer health checks active
- [ ] Ingress controller properly configured
- [ ] Auto-scaling policies activated

### **Post-deployment:**
- [ ] Health checks passing (API, UI, WebRTC)
- [ ] Metrics collecting properly in Prometheus
- [ ] Logs flowing to centralized system (Seq/ELK)
- [ ] Interview session creation/completion tested
- [ ] WebRTC connections establishing successfully
- [ ] Performance within acceptable limits
- [ ] AI cost tracking active and within budget
- [ ] Alerts configured and tested
- [ ] SSL certificates auto-renewal working
- [ ] Backup and disaster recovery tested

### **WebRTC-Specific Checks:**
- [ ] STUN/TURN servers accessible
- [ ] UDP port ranges open for media
- [ ] WebSocket connections for signaling working
- [ ] Recording functionality operational
- [ ] Connection quality monitoring active

This deployment strategy ensures reliable, scalable, and maintainable infrastructure for the AI Interview Preparation System with robust WebRTC support! 🚀