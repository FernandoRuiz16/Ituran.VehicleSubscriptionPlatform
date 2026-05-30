# Ituran Vehicle Subscription Platform

Plataforma desarrollada para procesar eventos de suscripciones vehiculares provenientes de webhooks externos, integrando procesos de ERP, CRM y monitoreo operativo.

---

## Arquitectura

```text
Webhook
   │
   ▼
API (.NET 8)
   │
   ▼
Worker Background Service
   │
   ├── ERP Integration
   │        └── Contract Creation
   │
   └── CRM Integration
            └── Mobile User Creation
   │
   ▼
SQL Server
   │
   ▼
Angular Dashboard
```

---

## Tecnologías

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- BackgroundService
- Dependency Injection

### Frontend

- Angular 21
- TypeScript
- SCSS
- Responsive Design

### Base de Datos

- SQL Server

---

## Funcionalidades

### Recepción de Webhooks

Recepción de eventos de suscripción vehicular.

Ejemplo:

```json
{
  "vin": "3N1AB7AP2HY123456",
  "placa": "ABC123",
  "dispositivo": "GPS-001",
  "eventoId": 101,
  "evento": "Alta de suscripcion",
  "nombre": "Luis",
  "apellidos": "Ruiz",
  "correo": "luis@test.com"
}
```

---

### Procesamiento Asíncrono

Los eventos son almacenados y posteriormente procesados por un Worker.

Proceso:

1. Validación
2. Generación de contrato ERP
3. Creación de usuario CRM
4. Actualización de estado

---

### Dashboard

Visualización de:

- Total de registros
- Completados
- Pendientes
- Procesando
- Fallidos
- Batches procesados

---

### Subscriptions

Listado completo de registros procesados:

- VIN
- Placa
- Dispositivo
- Evento
- Correo
- Estado
- Contrato ERP
- Usuario CRM

---

### Analytics

Métricas operativas:

- Total Batches
- Total Items
- Completed Items
- Failed Items
- ERP Steps
- CRM Steps
- Logs Generados

---

## Endpoints

### Dashboard

```http
GET /api/dashboard/summary
```

---

### Subscriptions

```http
GET /api/subscriptions
```

---

### Analytics

```http
GET /api/analytics/summary
```

---

### Webhook

```http
POST /api/webhook
```

---

## Configuración

### Backend

```bash
cd backend
dotnet restore
dotnet run
```

API:

```text
http://localhost:5005
```

---

### Frontend

```bash
cd frontend/ituran-subscriptions-app
npm install
ng serve
```

Aplicación:

```text
http://localhost:4200
```

---

## Flujo de Negocio

Cuando se recibe una nueva suscripción:

1. El webhook recibe el evento.
2. El evento se almacena en SQL Server.
3. El Worker procesa el registro.
4. Se genera el contrato ERP.
5. Se crea la cuenta CRM para la aplicación móvil.
6. Se actualiza el estado.
7. El Dashboard refleja los resultados.

---

## Mejoras Futuras

- JWT Authentication
- Role Based Access
- SignalR Real Time Updates
- Retry Policies
- RabbitMQ
- Azure Service Bus
- Docker Support
- Kubernetes Deployment

---

## Autor

Luis Fernando Ruiz

Backend Developer (.NET)