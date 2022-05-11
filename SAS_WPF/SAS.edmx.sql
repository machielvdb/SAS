
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/11/2022 18:39:46
-- Generated from EDMX file: C:\Users\Machiel\source\repos\SAS\SAS_WPF\SAS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SAS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DrinkOrder_Drink]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DrinkOrders] DROP CONSTRAINT [FK_DrinkOrder_Drink];
GO
IF OBJECT_ID(N'[dbo].[FK_DrinkOrder_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DrinkOrders] DROP CONSTRAINT [FK_DrinkOrder_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_Order_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Order_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DrinkOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DrinkOrders];
GO
IF OBJECT_ID(N'[dbo].[Drinks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Drinks];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DrinkOrders'
CREATE TABLE [dbo].[DrinkOrders] (
    [ID] uniqueidentifier  NOT NULL,
    [DrinkID] uniqueidentifier  NOT NULL,
    [OrderID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Drinks'
CREATE TABLE [dbo].[Drinks] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [IsBlocked] bit  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [ID] uniqueidentifier  NOT NULL,
    [Amount] int  NOT NULL,
    [FullDay] bit  NULL,
    [UserID] uniqueidentifier  NOT NULL,
    [WarmMeal] bit  NOT NULL,
    [Time] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] uniqueidentifier  NOT NULL,
    [UID] nvarchar(50)  NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Pincode] int  NULL,
    [Admin] bit  NOT NULL,
    [IsBlocked] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'DrinkOrders'
ALTER TABLE [dbo].[DrinkOrders]
ADD CONSTRAINT [PK_DrinkOrders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Drinks'
ALTER TABLE [dbo].[Drinks]
ADD CONSTRAINT [PK_Drinks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DrinkID] in table 'DrinkOrders'
ALTER TABLE [dbo].[DrinkOrders]
ADD CONSTRAINT [FK_DrinkOrder_Drink]
    FOREIGN KEY ([DrinkID])
    REFERENCES [dbo].[Drinks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrinkOrder_Drink'
CREATE INDEX [IX_FK_DrinkOrder_Drink]
ON [dbo].[DrinkOrders]
    ([DrinkID]);
GO

-- Creating foreign key on [OrderID] in table 'DrinkOrders'
ALTER TABLE [dbo].[DrinkOrders]
ADD CONSTRAINT [FK_DrinkOrder_Order]
    FOREIGN KEY ([OrderID])
    REFERENCES [dbo].[Orders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DrinkOrder_Order'
CREATE INDEX [IX_FK_DrinkOrder_Order]
ON [dbo].[DrinkOrders]
    ([OrderID]);
GO

-- Creating foreign key on [UserID] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Order_User]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Order_User'
CREATE INDEX [IX_FK_Order_User]
ON [dbo].[Orders]
    ([UserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------