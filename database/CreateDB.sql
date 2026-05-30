CREATE DATABASE Sol290526;
GO

--- Ceate tables

CREATE TABLE WebhookBatches (
    Id INT IDENTITY PRIMARY KEY,
    CorrelationId UNIQUEIDENTIFIER NOT NULL,
    Source NVARCHAR(100) NOT NULL,
    ReceivedAt DATETIME2 NOT NULL,
    TotalItems INT NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    RawJson NVARCHAR(MAX) NOT NULL,
    Signature NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE WebhookItems (
    Id INT IDENTITY PRIMARY KEY,
    BatchId INT NOT NULL,
    CorrelationId UNIQUEIDENTIFIER NOT NULL,
    Vin NVARCHAR(100) NOT NULL,
    Placa NVARCHAR(50) NULL,
    Dispositivo NVARCHAR(100) NULL,
    FechaEnvio DATETIME2 NULL,
    FechaInicio DATETIME2 NULL,
    FechaFinal DATETIME2 NULL,
    EventoId INT NULL,
    Evento NVARCHAR(200) NULL,
    Orden NVARCHAR(100) NULL,
    Nombre NVARCHAR(150) NULL,
    Apellidos NVARCHAR(150) NULL,
    Correo NVARCHAR(200) NULL,
    Status NVARCHAR(50) NOT NULL,
    RetryCount INT NOT NULL DEFAULT 0,
    IdempotencyKey NVARCHAR(300) NOT NULL,
    ErpContractId NVARCHAR(100) NULL,
    CrmUserId NVARCHAR(100) NULL,
    LastError NVARCHAR(MAX) NULL,
    LockedAt DATETIME2 NULL,
    ProcessedAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_WebhookItems_Batch FOREIGN KEY (BatchId) REFERENCES WebhookBatches(Id)
);

CREATE TABLE ProcessingLogs (
    Id INT IDENTITY PRIMARY KEY,
    WebhookItemId INT NOT NULL,
    CorrelationId UNIQUEIDENTIFIER NOT NULL,
    Step NVARCHAR(100) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Message NVARCHAR(MAX) NULL,
    ErrorDetail NVARCHAR(MAX) NULL,
    DurationMs INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_ProcessingLogs_Item FOREIGN KEY (WebhookItemId) REFERENCES WebhookItems(Id)
);

CREATE UNIQUE INDEX UX_WebhookItems_IdempotencyKey 
ON WebhookItems(IdempotencyKey);

ALTER TABLE WebhookBatches
ADD UpdatedAt DATETIME2 NULL;

ALTER TABLE WebhookItems
ADD UpdatedAt DATETIME2 NULL;

CREATE INDEX IX_WebhookItems_Status
ON WebhookItems(Status);

CREATE INDEX IX_WebhookItems_CorrelationId
ON WebhookItems(CorrelationId);

CREATE INDEX IX_ProcessingLogs_CorrelationId
ON ProcessingLogs(CorrelationId);