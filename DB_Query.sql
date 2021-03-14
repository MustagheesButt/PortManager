DROP TABLE IF EXISTS [user];
DROP TABLE IF EXISTS [trader];
DROP TABLE IF EXISTS [ship];
DROP TABLE IF EXISTS [item];
DROP TABLE IF EXISTS [items_ships];
DROP TABLE IF EXISTS [custom_duty];


CREATE TABLE [dbo].[user] (
    [Id]            INT         IDENTITY (1, 1) NOT NULL,
    [first_name]    VARCHAR (100) NULL,
    [last_name]     VARCHAR (100) NULL,
    [email]         VARCHAR (255) NULL,
    [password_hash] VARCHAR (100) NULL,
    [cnic]          VARCHAR (100) NULL,
    [user_type]     INT         NULL,
    [gender]        INT         NULL,
    [created_at]    DATETIME    NULL,
    [updated_at]    DATETIME    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[trader] (
    [user_id]         INT         NOT NULL,
    [nationality]     VARCHAR (100) NULL,
    [trading_number]  VARCHAR (100) NULL
);

CREATE TABLE [dbo].[ship] (
    [id]                  INT         IDENTITY (1, 1) NOT NULL,
    [hin]                 VARCHAR (100) NOT NULL,
    [trader_id]           INT NOT NULL,
    [nick_name]           VARCHAR (100) NULL,
    [allocated_birth]     INT NULL,
    [allocated_terminal]  INT NULL,
    [created_at] DATETIME NULL, 
    [updated_at] DATETIME NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[item] (
    [id]                  INT         IDENTITY (1, 1) NOT NULL,
    [name]                VARCHAR (255) NOT NULL,
    [price]               DECIMAL NOT NULL DEFAULT 0,
    [currency]            VARCHAR(10) NOT NULL DEFAULT 'PKR',
    [manufacturer]        VARCHAR(100) NULL,
    [trader_id]           INT NOT NULL,
    [created_at] DATETIME NULL,
    [updated_at] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[items_ships] (
    [item_id]             INT NOT NULL,
    [ship_id]             INT NOT NULL,
    [quantity]            INT NOT NULL DEFAULT 0,
    [created_at] DATETIME NULL, 
    [updated_at] DATETIME NULL
);

CREATE TABLE [dbo].[custom_duty] (
    [id]                  INT         IDENTITY (1, 1) NOT NULL,
    [amount]              DECIMAL NOT NULL DEFAULT 0,
    [currency]            VARCHAR(10) NOT NULL DEFAULT 'PKR',
    [ship_id]             INT NOT NULL,
    [status]              VARCHAR(10) NOT NULL DEFAULT 'Unpaid',
    [due_date]   DATETIME NULL,
    [paid_at]    DATETIME NULL,
    [created_at] DATETIME NULL,
    [updated_at] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


-- Dummy data
INSERT INTO [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) VALUES ('Mr.', 'Admin', 'admin@gmail.com', 'password', '123-123-1', 0, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) VALUES ('Henry', 'Trader', 'henry@gmail.com', 'password', '123-123-2', 1, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) VALUES ('Mark', 'Trader', 'mark@gmail.com', 'password', '123-123-3', 1, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) VALUES ('Lisa', 'Staff', 'lisa@gmail.com', 'password', '123-123-4', 2, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO [trader] (user_id, nationality, trading_number) VALUES (2, 'Korea', '1231232');
INSERT INTO [trader] (user_id, nationality, trading_number) VALUES (3, 'England', '5231252');

INSERT INTO [ship] (hin, trader_id, nick_name, allocated_birth, allocated_terminal, created_at, updated_at) VALUES ('11223344', 2, 'Nezuco Victory', 1, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [ship] (hin, trader_id, nick_name, allocated_birth, allocated_terminal, created_at, updated_at) VALUES ('33223345', 3, 'Ark MSS', 1, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [ship] (hin, trader_id, nick_name, allocated_birth, allocated_terminal, created_at, updated_at) VALUES ('22223346', 2, 'Fortune''s End', 2, 6, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO [item] (name, price, currency, manufacturer, trader_id, created_at, updated_at) VALUES ('GTX 1660S', 30000, 'PKR', 'Nvidia', 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [item] (name, price, currency, manufacturer, trader_id, created_at, updated_at) VALUES ('Ryzen 5 3600X', 45000, 'PKR', 'AMD', 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [items_ships] (item_id, ship_id, quantity, created_at, updated_at) VALUES (1, 1, 100, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [items_ships] (item_id, ship_id, quantity, created_at, updated_at) VALUES (2, 1, 50, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

INSERT INTO [item] (name, price, currency, manufacturer, trader_id, created_at, updated_at) VALUES ('RTX 3060', 50000, 'PKR', 'Nvidia', 3, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO [items_ships] (item_id, ship_id, quantity, created_at, updated_at) VALUES (3, 2, 99, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);