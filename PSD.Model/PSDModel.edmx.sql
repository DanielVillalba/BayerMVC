
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/20/2017 12:48:16
-- Generated from EDMX file: C:\Users\vargasje\Documents\Employee\lph\RoomieIT\PortalBayer\PSD\PSD_TFS\PSD\PSD.Model\PSDModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PSDDB_V1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Cat_UserRoleRolesXUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolesXUser] DROP CONSTRAINT [FK_Cat_UserRoleRolesXUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRolesXUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolesXUser] DROP CONSTRAINT [FK_UserRolesXUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_UserStatusUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_Cat_UserStatusUser];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_PersonUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressStateAddressMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressMunicipality] DROP CONSTRAINT [FK_AddressStateAddressMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorDistributorUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_DistributorEmployee] DROP CONSTRAINT [FK_DistributorDistributorUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressMunicipalityAddressPostalCode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressPostalCode] DROP CONSTRAINT [FK_AddressMunicipalityAddressPostalCode];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressPostalCodeAddressColony]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressColony] DROP CONSTRAINT [FK_AddressPostalCodeAddressColony];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressColonyAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_AddressColonyAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Distributor] DROP CONSTRAINT [FK_AddressDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_DistributorContactAreaDistributorContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorContact] DROP CONSTRAINT [FK_Cat_DistributorContactAreaDistributorContact];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorDistributorContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorContact] DROP CONSTRAINT [FK_DistributorDistributorContact];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressDistributorBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorBranch] DROP CONSTRAINT [FK_AddressDistributorBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_DistributorBranchContactAreaDistributorBranchContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorBranchContact] DROP CONSTRAINT [FK_Cat_DistributorBranchContactAreaDistributorBranchContact];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorBranchDistributorBranchContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorBranchContact] DROP CONSTRAINT [FK_DistributorBranchDistributorBranchContact];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subdistributor] DROP CONSTRAINT [FK_AddressSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_ZoneAddressMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressMunicipality] DROP CONSTRAINT [FK_Cat_ZoneAddressMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorSubdistributorEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_SubdistributorEmployee] DROP CONSTRAINT [FK_SubdistributorSubdistributorEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressAddressesXDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressesXSubdistributor] DROP CONSTRAINT [FK_AddressAddressesXDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subdistributor] DROP CONSTRAINT [FK_BayerEmployeeSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeSubdistributor1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subdistributor] DROP CONSTRAINT [FK_BayerEmployeeSubdistributor1];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorSubdistributorContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorContact] DROP CONSTRAINT [FK_SubdistributorSubdistributorContact];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_SubdistributorContactAreaSubdistributorContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorContact] DROP CONSTRAINT [FK_Cat_SubdistributorContactAreaSubdistributorContact];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_CropSubdistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorCropsXMunicipality] DROP CONSTRAINT [FK_Cat_CropSubdistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressMunicipalitySubdistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorCropsXMunicipality] DROP CONSTRAINT [FK_AddressMunicipalitySubdistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorSubdistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorCropsXMunicipality] DROP CONSTRAINT [FK_SubdistributorSubdistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressMunicipalityDistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorCropsXMunicipality] DROP CONSTRAINT [FK_AddressMunicipalityDistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_CropDistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorCropsXMunicipality] DROP CONSTRAINT [FK_Cat_CropDistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeMunicipalitiesXEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MunicipalitiesXEmployee] DROP CONSTRAINT [FK_BayerEmployeeMunicipalitiesXEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressMunicipalityMunicipalitiesXEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MunicipalitiesXEmployee] DROP CONSTRAINT [FK_AddressMunicipalityMunicipalitiesXEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorSubdistributorCommercialName]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubdistributorCommercialName] DROP CONSTRAINT [FK_SubdistributorSubdistributorCommercialName];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorDistributorCropsXMunicipality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorCropsXMunicipality] DROP CONSTRAINT [FK_DistributorDistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorDistributorBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorBranch] DROP CONSTRAINT [FK_DistributorDistributorBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorContractDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractDistributor] DROP CONSTRAINT [FK_DistributorContractDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeContractDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractDistributor] DROP CONSTRAINT [FK_BayerEmployeeContractDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeContractDistributor1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractDistributor] DROP CONSTRAINT [FK_BayerEmployeeContractDistributor1];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_ContractDistributorStatusContractDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractDistributor] DROP CONSTRAINT [FK_Cat_ContractDistributorStatusContractDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_CropCategoryCat_Crop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cat_Crop] DROP CONSTRAINT [FK_Cat_CropCategoryCat_Crop];
GO
IF OBJECT_ID(N'[dbo].[FK_Cat_ContractSubdistributorStatusContractSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_Cat_ContractSubdistributorStatusContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorContractSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_SubdistributorContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeContractSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_BayerEmployeeContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployeeContractSubdistributor1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_BayerEmployeeContractSubdistributor1];
GO
IF OBJECT_ID(N'[dbo].[FK_NewsNewsSection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NewsSection] DROP CONSTRAINT [FK_NewsNewsSection];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorAddressesXSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressesXSubdistributor] DROP CONSTRAINT [FK_SubdistributorAddressesXSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractSubdistributorDistributorPurchasesXContractSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorPurchasesXContractSubdistributor] DROP CONSTRAINT [FK_ContractSubdistributorDistributorPurchasesXContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorDistributorPurchasesXContractSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DistributorPurchasesXContractSubdistributor] DROP CONSTRAINT [FK_DistributorDistributorPurchasesXContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractDistributorDistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Distributor] DROP CONSTRAINT [FK_ContractDistributorDistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractSubdistributorSubdistributor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subdistributor] DROP CONSTRAINT [FK_ContractSubdistributorSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractSubdistributorSubdistributorDiscountCoupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_ContractSubdistributorSubdistributorDiscountCoupon];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractSubdistributorSubdistributorPromotionCoupon]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractSubdistributor] DROP CONSTRAINT [FK_ContractSubdistributorSubdistributorPromotionCoupon];
GO
IF OBJECT_ID(N'[dbo].[FK_DistributorEmployee_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_DistributorEmployee] DROP CONSTRAINT [FK_DistributorEmployee_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_SubdistributorEmployee_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_SubdistributorEmployee] DROP CONSTRAINT [FK_SubdistributorEmployee_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_BayerEmployee_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_BayerEmployee] DROP CONSTRAINT [FK_BayerEmployee_inherits_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[RolesXUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolesXUser];
GO
IF OBJECT_ID(N'[dbo].[Cat_UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_UserRole];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Cat_UserStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_UserStatus];
GO
IF OBJECT_ID(N'[dbo].[Distributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Distributor];
GO
IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[AddressMunicipality]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressMunicipality];
GO
IF OBJECT_ID(N'[dbo].[AddressState]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressState];
GO
IF OBJECT_ID(N'[dbo].[AddressColony]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressColony];
GO
IF OBJECT_ID(N'[dbo].[AddressPostalCode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressPostalCode];
GO
IF OBJECT_ID(N'[dbo].[SubdistributorCommercialName]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubdistributorCommercialName];
GO
IF OBJECT_ID(N'[dbo].[Cat_Zone]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_Zone];
GO
IF OBJECT_ID(N'[dbo].[Cat_Crop]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_Crop];
GO
IF OBJECT_ID(N'[dbo].[DistributorContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DistributorContact];
GO
IF OBJECT_ID(N'[dbo].[Cat_DistributorContactArea]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_DistributorContactArea];
GO
IF OBJECT_ID(N'[dbo].[DistributorBranch]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DistributorBranch];
GO
IF OBJECT_ID(N'[dbo].[DistributorBranchContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DistributorBranchContact];
GO
IF OBJECT_ID(N'[dbo].[Cat_DistributorBranchContactArea]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_DistributorBranchContactArea];
GO
IF OBJECT_ID(N'[dbo].[Subdistributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subdistributor];
GO
IF OBJECT_ID(N'[dbo].[AddressesXSubdistributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressesXSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[SubdistributorContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubdistributorContact];
GO
IF OBJECT_ID(N'[dbo].[Cat_SubdistributorContactArea]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_SubdistributorContactArea];
GO
IF OBJECT_ID(N'[dbo].[SubdistributorCropsXMunicipality]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubdistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[DistributorCropsXMunicipality]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DistributorCropsXMunicipality];
GO
IF OBJECT_ID(N'[dbo].[MunicipalitiesXEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MunicipalitiesXEmployee];
GO
IF OBJECT_ID(N'[dbo].[ContractDistributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractDistributor];
GO
IF OBJECT_ID(N'[dbo].[Cat_ContractDistributorStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_ContractDistributorStatus];
GO
IF OBJECT_ID(N'[dbo].[Cat_CropCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_CropCategory];
GO
IF OBJECT_ID(N'[dbo].[ContentLink]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentLink];
GO
IF OBJECT_ID(N'[dbo].[AppConfiguration]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppConfiguration];
GO
IF OBJECT_ID(N'[dbo].[ContractSubdistributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[Cat_ContractSubdistributorStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cat_ContractSubdistributorStatus];
GO
IF OBJECT_ID(N'[dbo].[DistributorPurchasesXContractSubdistributor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DistributorPurchasesXContractSubdistributor];
GO
IF OBJECT_ID(N'[dbo].[News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[News];
GO
IF OBJECT_ID(N'[dbo].[NewsSection]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsSection];
GO
IF OBJECT_ID(N'[dbo].[ContentData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentData];
GO
IF OBJECT_ID(N'[dbo].[SubdistributorDiscountCoupon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubdistributorDiscountCoupon];
GO
IF OBJECT_ID(N'[dbo].[SubdistributorPromotionCoupon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubdistributorPromotionCoupon];
GO
IF OBJECT_ID(N'[dbo].[Person_DistributorEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_DistributorEmployee];
GO
IF OBJECT_ID(N'[dbo].[Person_SubdistributorEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_SubdistributorEmployee];
GO
IF OBJECT_ID(N'[dbo].[Person_BayerEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_BayerEmployee];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cat_UserStatusId] int  NOT NULL,
    [NickName] nvarchar(max)  NOT NULL,
    [Salt] nvarchar(max)  NULL,
    [Hash] nvarchar(max)  NULL,
    [LastLoginDate] datetime  NULL,
    [FailedLoginAttempts] int  NOT NULL,
    [LastPasswordChangeDate] datetime  NULL,
    [LoginToken] nvarchar(max)  NULL,
    [LoginTokenGeneratedDate] datetime  NULL
);
GO

-- Creating table 'RolesXUser'
CREATE TABLE [dbo].[RolesXUser] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cat_UserRoleId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Cat_UserRole'
CREATE TABLE [dbo].[Cat_UserRole] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [LastNameF] nvarchar(max)  NULL,
    [LastNameM] nvarchar(max)  NULL,
    [EMail] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'Cat_UserStatus'
CREATE TABLE [dbo].[Cat_UserStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Distributor'
CREATE TABLE [dbo].[Distributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [BusinessName] nvarchar(max)  NOT NULL,
    [CommercialName] nvarchar(max)  NOT NULL,
    [AddressId] int  NULL,
    [WebSite] nvarchar(max)  NULL,
    [GeolocationLongitude] nvarchar(max)  NULL,
    [GeolocationLatitude] nvarchar(max)  NULL,
    [CurrentContract_Id] int  NULL
);
GO

-- Creating table 'Address'
CREATE TABLE [dbo].[Address] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [NumberExt] nvarchar(max)  NULL,
    [NumberInt] nvarchar(max)  NULL,
    [AddressColonyId] int  NULL,
    [AddressPostalCodeId] int  NULL,
    [AddressMunicipalityId] int  NULL,
    [AddressStateId] int  NULL
);
GO

-- Creating table 'AddressMunicipality'
CREATE TABLE [dbo].[AddressMunicipality] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AddressStateId] int  NOT NULL,
    [Cat_ZoneId] int  NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AddressState'
CREATE TABLE [dbo].[AddressState] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AddressColony'
CREATE TABLE [dbo].[AddressColony] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AddressPostalCodeId] int  NOT NULL,
    [AddressMunicipalityId] int  NOT NULL,
    [AddressStateId] int  NOT NULL
);
GO

-- Creating table 'AddressPostalCode'
CREATE TABLE [dbo].[AddressPostalCode] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AddressMunicipalityId] int  NOT NULL,
    [AddressStateId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubdistributorCommercialName'
CREATE TABLE [dbo].[SubdistributorCommercialName] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubdistributorId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsMain] bit  NOT NULL
);
GO

-- Creating table 'Cat_Zone'
CREATE TABLE [dbo].[Cat_Zone] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [RegionName] nvarchar(max)  NULL
);
GO

-- Creating table 'Cat_Crop'
CREATE TABLE [dbo].[Cat_Crop] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CropCategoryId] int  NOT NULL
);
GO

-- Creating table 'DistributorContact'
CREATE TABLE [dbo].[DistributorContact] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DistributorContactAreaId] int  NULL,
    [DistributorId] int  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberExt] nvarchar(max)  NULL,
    [CellPhone] nvarchar(max)  NULL,
    [EMail] nvarchar(max)  NOT NULL,
    [AssistantFullName] nvarchar(max)  NULL,
    [AssistantPhoneNumber] nvarchar(max)  NULL,
    [AssistantPhoneNumberExt] nvarchar(max)  NULL,
    [AssistantCellPhone] nvarchar(max)  NULL,
    [AssistantEMail] nvarchar(max)  NULL
);
GO

-- Creating table 'Cat_DistributorContactArea'
CREATE TABLE [dbo].[Cat_DistributorContactArea] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DistributorBranch'
CREATE TABLE [dbo].[DistributorBranch] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DistributorId] int  NOT NULL,
    [AddressId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [GeolocationLongitude] nvarchar(max)  NULL,
    [GeolocationLatitude] nvarchar(max)  NULL
);
GO

-- Creating table 'DistributorBranchContact'
CREATE TABLE [dbo].[DistributorBranchContact] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DistributorBranchId] int  NOT NULL,
    [Cat_DistributorBranchContactAreaId] int  NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberExt] nvarchar(max)  NULL,
    [CellPhone] nvarchar(max)  NULL,
    [EMail] nvarchar(max)  NOT NULL,
    [AssistantFullName] nvarchar(max)  NULL,
    [AssistantPhoneNumber] nvarchar(max)  NULL,
    [AssistantPhoneNumberExt] nvarchar(max)  NULL,
    [AssistantCellPhone] nvarchar(max)  NULL,
    [AssistantEMail] nvarchar(max)  NULL
);
GO

-- Creating table 'Cat_DistributorBranchContactArea'
CREATE TABLE [dbo].[Cat_DistributorBranchContactArea] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Subdistributor'
CREATE TABLE [dbo].[Subdistributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [RTVCreator_BayerEmployeeId] int  NOT NULL,
    [RTV_BayerEmployeeId] int  NULL,
    [Type] nvarchar(max)  NOT NULL,
    [BusinessName] nvarchar(max)  NOT NULL,
    [BNLegalRepresentative] nvarchar(max)  NOT NULL,
    [BNAddressId] int  NOT NULL,
    [WebSite] nvarchar(max)  NULL,
    [GeolocationLatitude] nvarchar(max)  NULL,
    [GeolocationLongitude] nvarchar(max)  NULL,
    [LastContractTotalGoal] decimal(18,0)  NOT NULL,
    [LastContractTotalPurchased] decimal(18,0)  NOT NULL,
    [CurrentContract_Id] int  NULL
);
GO

-- Creating table 'AddressesXSubdistributor'
CREATE TABLE [dbo].[AddressesXSubdistributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AddressId] int  NOT NULL,
    [SubdistributorId] int  NOT NULL
);
GO

-- Creating table 'SubdistributorContact'
CREATE TABLE [dbo].[SubdistributorContact] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubdistributorId] int  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Role] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberExt] nvarchar(max)  NULL,
    [CellPhone] nvarchar(max)  NULL,
    [EMail] nvarchar(max)  NOT NULL,
    [AssistantFullName] nvarchar(max)  NULL,
    [AssistantPhoneNumber] nvarchar(max)  NULL,
    [AssistantPhoneNumberExt] nvarchar(max)  NULL,
    [AssistantCellPhone] nvarchar(max)  NULL,
    [AssistantEMail] nvarchar(max)  NULL,
    [Cat_SubdistributorContactAreaId] int  NULL
);
GO

-- Creating table 'Cat_SubdistributorContactArea'
CREATE TABLE [dbo].[Cat_SubdistributorContactArea] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubdistributorCropsXMunicipality'
CREATE TABLE [dbo].[SubdistributorCropsXMunicipality] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cat_CropId] int  NOT NULL,
    [AddressMunicipalityId] int  NOT NULL,
    [AddressMunicipalityAddressStateId] int  NOT NULL,
    [SubdistributorId] int  NOT NULL
);
GO

-- Creating table 'DistributorCropsXMunicipality'
CREATE TABLE [dbo].[DistributorCropsXMunicipality] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AddressMunicipalityId] int  NOT NULL,
    [AddressMunicipalityAddressStateId] int  NOT NULL,
    [Cat_CropId] int  NOT NULL,
    [DistributorId] int  NOT NULL
);
GO

-- Creating table 'MunicipalitiesXEmployee'
CREATE TABLE [dbo].[MunicipalitiesXEmployee] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BayerEmployeeId] int  NOT NULL,
    [AddressMunicipalityId] int  NOT NULL,
    [AddressMunicipalityAddressStateId] int  NOT NULL
);
GO

-- Creating table 'ContractDistributor'
CREATE TABLE [dbo].[ContractDistributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [ContractDistributorStatusId] int  NOT NULL,
    [DistributorId] int  NOT NULL,
    [GRVBayerEmployeeId] int  NOT NULL,
    [RTVBayerEmployeeId] int  NOT NULL,
    [Year] int  NOT NULL,
    [ContractDate] datetime  NULL,
    [RegisteredZoneName] nvarchar(max)  NOT NULL,
    [RegisteredRegionName] nvarchar(max)  NULL,
    [DiscountType] int  NOT NULL,
    [ContractFilePath] nvarchar(max)  NULL,
    [AmountGoalQ1Pre] decimal(18,0)  NOT NULL,
    [AmountGoalQ1] decimal(18,0)  NOT NULL,
    [AmountGoalQ2Pre] decimal(18,0)  NOT NULL,
    [AmountGoalQ2] decimal(18,0)  NOT NULL,
    [AmountGoalQ3Pre] decimal(18,0)  NOT NULL,
    [AmountGoalQ3] decimal(18,0)  NOT NULL,
    [AmountGoalQ4Pre] decimal(18,0)  NOT NULL,
    [AmountGoalQ4] decimal(18,0)  NOT NULL,
    [AmountGoalTotalPre] decimal(18,0)  NOT NULL,
    [AmountGoalTotal] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Cat_ContractDistributorStatus'
CREATE TABLE [dbo].[Cat_ContractDistributorStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Cat_CropCategory'
CREATE TABLE [dbo].[Cat_CropCategory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ContentLink'
CREATE TABLE [dbo].[ContentLink] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DisplayName] nvarchar(max)  NOT NULL,
    [Url] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AppConfiguration'
CREATE TABLE [dbo].[AppConfiguration] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ContractSubdistributor'
CREATE TABLE [dbo].[ContractSubdistributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [ContractSubdistributorStatusId] int  NOT NULL,
    [SubdistributorId] int  NOT NULL,
    [GRVBayerEmployeeId] int  NOT NULL,
    [RTVBayerEmployeeId] int  NOT NULL,
    [Year] int  NOT NULL,
    [ContractDate] datetime  NULL,
    [RegisteredZoneName] nvarchar(max)  NOT NULL,
    [RegisteredRegionName] nvarchar(max)  NULL,
    [ContractFilePath] nvarchar(max)  NULL,
    [DiscountType] int  NOT NULL,
    [AmountGoalS1Pre] decimal(18,0)  NOT NULL,
    [AmountGoalS1] decimal(18,0)  NOT NULL,
    [AmountGoalS2Pre] decimal(18,0)  NOT NULL,
    [AmountGoalS2] decimal(18,0)  NOT NULL,
    [AmountGoalTotalPre] decimal(18,0)  NOT NULL,
    [AmountGoalTotal] decimal(18,0)  NOT NULL,
    [PurchaseTotalJan] decimal(18,0)  NOT NULL,
    [PurchaseTotalFeb] decimal(18,0)  NOT NULL,
    [PurchaseTotalMar] decimal(18,0)  NOT NULL,
    [PurchaseTotalApr] decimal(18,0)  NOT NULL,
    [PurchaseTotalMay] decimal(18,0)  NOT NULL,
    [PurchaseTotalJun] decimal(18,0)  NOT NULL,
    [PurchaseTotalJul] decimal(18,0)  NOT NULL,
    [PurchaseTotalAgo] decimal(18,0)  NOT NULL,
    [PurchaseTotalSep] decimal(18,0)  NOT NULL,
    [PurchaseTotalOct] decimal(18,0)  NOT NULL,
    [PurchaseTotalNov] decimal(18,0)  NOT NULL,
    [PurchaseTotalDic] decimal(18,0)  NOT NULL,
    [PurchaseTotalS1] decimal(18,0)  NOT NULL,
    [PurchaseTotalS2] decimal(18,0)  NOT NULL,
    [PurchaseTotal] decimal(18,0)  NOT NULL,
    [SubdistributorDiscountCoupon_Id] int  NOT NULL,
    [SubdistributorPromotionCoupon_Id] int  NOT NULL
);
GO

-- Creating table 'Cat_ContractSubdistributorStatus'
CREATE TABLE [dbo].[Cat_ContractSubdistributorStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IdB] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DistributorPurchasesXContractSubdistributor'
CREATE TABLE [dbo].[DistributorPurchasesXContractSubdistributor] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContractSubdistributorId] int  NOT NULL,
    [DistributorId] int  NOT NULL,
    [PurchaseJan] decimal(18,0)  NOT NULL,
    [PurchaseFeb] decimal(18,0)  NOT NULL,
    [PurchaseMar] decimal(18,0)  NOT NULL,
    [PurchaseApr] decimal(18,0)  NOT NULL,
    [PurchaseMay] decimal(18,0)  NOT NULL,
    [PurchaseJun] decimal(18,0)  NOT NULL,
    [PurchaseJul] decimal(18,0)  NOT NULL,
    [PurchaseAgo] decimal(18,0)  NOT NULL,
    [PurchaseSep] decimal(18,0)  NOT NULL,
    [PurchaseOct] decimal(18,0)  NOT NULL,
    [PurchaseNov] decimal(18,0)  NOT NULL,
    [PurchaseDic] decimal(18,0)  NOT NULL,
    [PurchaseTotal] decimal(18,0)  NOT NULL,
    [PurchaseTotalS1] decimal(18,0)  NOT NULL,
    [PurchaseTotalS2] decimal(18,0)  NOT NULL,
    [CouponSharePercentage] decimal(18,0)  NOT NULL,
    [CouponSharePercentageS1] decimal(18,0)  NOT NULL,
    [CouponSharePercentageS2] decimal(18,0)  NOT NULL,
    [CouponShareAmount] decimal(18,0)  NOT NULL,
    [CouponShareAmountS1] decimal(18,0)  NOT NULL,
    [CouponShareAmountS2] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'News'
CREATE TABLE [dbo].[News] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [URLId] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Subtitle] nvarchar(max)  NOT NULL,
    [Paragraph] nvarchar(max)  NOT NULL,
    [Image] nvarchar(256)  NOT NULL,
    [ImageFooter] nvarchar(max)  NOT NULL,
    [Author] nvarchar(max)  NOT NULL,
    [IsDistributorVisible] bit  NOT NULL,
    [IsSubdistributorVisible] bit  NOT NULL,
    [IsFarmerVisible] bit  NOT NULL,
    [IsPublished] bit  NOT NULL,
    [IsNonAuthenticatedVisible] bit  NOT NULL,
    [PublishDate] datetime  NULL
);
GO

-- Creating table 'NewsSection'
CREATE TABLE [dbo].[NewsSection] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Paragraph] nvarchar(max)  NOT NULL,
    [Image] nvarchar(256)  NOT NULL,
    [ImageFooter] nvarchar(max)  NOT NULL,
    [NewsId] int  NOT NULL
);
GO

-- Creating table 'ContentData'
CREATE TABLE [dbo].[ContentData] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubdistributorDiscountCoupon'
CREATE TABLE [dbo].[SubdistributorDiscountCoupon] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CouponAmount] decimal(18,0)  NOT NULL,
    [CouponAmountS1] decimal(18,0)  NOT NULL,
    [CouponAmountS2] decimal(18,0)  NOT NULL,
    [HasCoupon] bit  NOT NULL,
    [HasCouponS1] bit  NOT NULL,
    [HasCouponS2] bit  NOT NULL,
    [IsCalculated] bit  NOT NULL,
    [IsCalculatedS1] bit  NOT NULL,
    [IsCalculatedS2] bit  NOT NULL
);
GO

-- Creating table 'SubdistributorPromotionCoupon'
CREATE TABLE [dbo].[SubdistributorPromotionCoupon] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CouponAmount] decimal(18,0)  NOT NULL,
    [CouponAmountS1] decimal(18,0)  NOT NULL,
    [CouponAmountS2] decimal(18,0)  NOT NULL,
    [HasCoupon] bit  NOT NULL,
    [HasCouponS1] bit  NOT NULL,
    [HasCouponS2] bit  NOT NULL,
    [IsCalculated] bit  NOT NULL,
    [IsCalculatedS1] bit  NOT NULL,
    [IsCalculatedS2] bit  NOT NULL
);
GO

-- Creating table 'Person_DistributorEmployee'
CREATE TABLE [dbo].[Person_DistributorEmployee] (
    [DistributorId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Person_SubdistributorEmployee'
CREATE TABLE [dbo].[Person_SubdistributorEmployee] (
    [SubdistributorId] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Person_BayerEmployee'
CREATE TABLE [dbo].[Person_BayerEmployee] (
    [IdB] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolesXUser'
ALTER TABLE [dbo].[RolesXUser]
ADD CONSTRAINT [PK_RolesXUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_UserRole'
ALTER TABLE [dbo].[Cat_UserRole]
ADD CONSTRAINT [PK_Cat_UserRole]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [PK_Person]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_UserStatus'
ALTER TABLE [dbo].[Cat_UserStatus]
ADD CONSTRAINT [PK_Cat_UserStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Distributor'
ALTER TABLE [dbo].[Distributor]
ADD CONSTRAINT [PK_Distributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [PK_Address]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [AddressStateId] in table 'AddressMunicipality'
ALTER TABLE [dbo].[AddressMunicipality]
ADD CONSTRAINT [PK_AddressMunicipality]
    PRIMARY KEY CLUSTERED ([Id], [AddressStateId] ASC);
GO

-- Creating primary key on [Id] in table 'AddressState'
ALTER TABLE [dbo].[AddressState]
ADD CONSTRAINT [PK_AddressState]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId] in table 'AddressColony'
ALTER TABLE [dbo].[AddressColony]
ADD CONSTRAINT [PK_AddressColony]
    PRIMARY KEY CLUSTERED ([Id], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId] ASC);
GO

-- Creating primary key on [Id], [AddressMunicipalityId], [AddressStateId] in table 'AddressPostalCode'
ALTER TABLE [dbo].[AddressPostalCode]
ADD CONSTRAINT [PK_AddressPostalCode]
    PRIMARY KEY CLUSTERED ([Id], [AddressMunicipalityId], [AddressStateId] ASC);
GO

-- Creating primary key on [Id] in table 'SubdistributorCommercialName'
ALTER TABLE [dbo].[SubdistributorCommercialName]
ADD CONSTRAINT [PK_SubdistributorCommercialName]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_Zone'
ALTER TABLE [dbo].[Cat_Zone]
ADD CONSTRAINT [PK_Cat_Zone]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_Crop'
ALTER TABLE [dbo].[Cat_Crop]
ADD CONSTRAINT [PK_Cat_Crop]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DistributorContact'
ALTER TABLE [dbo].[DistributorContact]
ADD CONSTRAINT [PK_DistributorContact]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_DistributorContactArea'
ALTER TABLE [dbo].[Cat_DistributorContactArea]
ADD CONSTRAINT [PK_Cat_DistributorContactArea]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DistributorBranch'
ALTER TABLE [dbo].[DistributorBranch]
ADD CONSTRAINT [PK_DistributorBranch]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DistributorBranchContact'
ALTER TABLE [dbo].[DistributorBranchContact]
ADD CONSTRAINT [PK_DistributorBranchContact]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_DistributorBranchContactArea'
ALTER TABLE [dbo].[Cat_DistributorBranchContactArea]
ADD CONSTRAINT [PK_Cat_DistributorBranchContactArea]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subdistributor'
ALTER TABLE [dbo].[Subdistributor]
ADD CONSTRAINT [PK_Subdistributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AddressesXSubdistributor'
ALTER TABLE [dbo].[AddressesXSubdistributor]
ADD CONSTRAINT [PK_AddressesXSubdistributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubdistributorContact'
ALTER TABLE [dbo].[SubdistributorContact]
ADD CONSTRAINT [PK_SubdistributorContact]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_SubdistributorContactArea'
ALTER TABLE [dbo].[Cat_SubdistributorContactArea]
ADD CONSTRAINT [PK_Cat_SubdistributorContactArea]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubdistributorCropsXMunicipality'
ALTER TABLE [dbo].[SubdistributorCropsXMunicipality]
ADD CONSTRAINT [PK_SubdistributorCropsXMunicipality]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DistributorCropsXMunicipality'
ALTER TABLE [dbo].[DistributorCropsXMunicipality]
ADD CONSTRAINT [PK_DistributorCropsXMunicipality]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MunicipalitiesXEmployee'
ALTER TABLE [dbo].[MunicipalitiesXEmployee]
ADD CONSTRAINT [PK_MunicipalitiesXEmployee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContractDistributor'
ALTER TABLE [dbo].[ContractDistributor]
ADD CONSTRAINT [PK_ContractDistributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_ContractDistributorStatus'
ALTER TABLE [dbo].[Cat_ContractDistributorStatus]
ADD CONSTRAINT [PK_Cat_ContractDistributorStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_CropCategory'
ALTER TABLE [dbo].[Cat_CropCategory]
ADD CONSTRAINT [PK_Cat_CropCategory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContentLink'
ALTER TABLE [dbo].[ContentLink]
ADD CONSTRAINT [PK_ContentLink]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AppConfiguration'
ALTER TABLE [dbo].[AppConfiguration]
ADD CONSTRAINT [PK_AppConfiguration]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [PK_ContractSubdistributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cat_ContractSubdistributorStatus'
ALTER TABLE [dbo].[Cat_ContractSubdistributorStatus]
ADD CONSTRAINT [PK_Cat_ContractSubdistributorStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DistributorPurchasesXContractSubdistributor'
ALTER TABLE [dbo].[DistributorPurchasesXContractSubdistributor]
ADD CONSTRAINT [PK_DistributorPurchasesXContractSubdistributor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'News'
ALTER TABLE [dbo].[News]
ADD CONSTRAINT [PK_News]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NewsSection'
ALTER TABLE [dbo].[NewsSection]
ADD CONSTRAINT [PK_NewsSection]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContentData'
ALTER TABLE [dbo].[ContentData]
ADD CONSTRAINT [PK_ContentData]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubdistributorDiscountCoupon'
ALTER TABLE [dbo].[SubdistributorDiscountCoupon]
ADD CONSTRAINT [PK_SubdistributorDiscountCoupon]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubdistributorPromotionCoupon'
ALTER TABLE [dbo].[SubdistributorPromotionCoupon]
ADD CONSTRAINT [PK_SubdistributorPromotionCoupon]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_DistributorEmployee'
ALTER TABLE [dbo].[Person_DistributorEmployee]
ADD CONSTRAINT [PK_Person_DistributorEmployee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_SubdistributorEmployee'
ALTER TABLE [dbo].[Person_SubdistributorEmployee]
ADD CONSTRAINT [PK_Person_SubdistributorEmployee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person_BayerEmployee'
ALTER TABLE [dbo].[Person_BayerEmployee]
ADD CONSTRAINT [PK_Person_BayerEmployee]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Cat_UserRoleId] in table 'RolesXUser'
ALTER TABLE [dbo].[RolesXUser]
ADD CONSTRAINT [FK_Cat_UserRoleRolesXUser]
    FOREIGN KEY ([Cat_UserRoleId])
    REFERENCES [dbo].[Cat_UserRole]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_UserRoleRolesXUser'
CREATE INDEX [IX_FK_Cat_UserRoleRolesXUser]
ON [dbo].[RolesXUser]
    ([Cat_UserRoleId]);
GO

-- Creating foreign key on [UserId] in table 'RolesXUser'
ALTER TABLE [dbo].[RolesXUser]
ADD CONSTRAINT [FK_UserRolesXUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRolesXUser'
CREATE INDEX [IX_FK_UserRolesXUser]
ON [dbo].[RolesXUser]
    ([UserId]);
GO

-- Creating foreign key on [Cat_UserStatusId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [FK_Cat_UserStatusUser]
    FOREIGN KEY ([Cat_UserStatusId])
    REFERENCES [dbo].[Cat_UserStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_UserStatusUser'
CREATE INDEX [IX_FK_Cat_UserStatusUser]
ON [dbo].[User]
    ([Cat_UserStatusId]);
GO

-- Creating foreign key on [User_Id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [FK_PersonUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonUser'
CREATE INDEX [IX_FK_PersonUser]
ON [dbo].[Person]
    ([User_Id]);
GO

-- Creating foreign key on [AddressStateId] in table 'AddressMunicipality'
ALTER TABLE [dbo].[AddressMunicipality]
ADD CONSTRAINT [FK_AddressStateAddressMunicipality]
    FOREIGN KEY ([AddressStateId])
    REFERENCES [dbo].[AddressState]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressStateAddressMunicipality'
CREATE INDEX [IX_FK_AddressStateAddressMunicipality]
ON [dbo].[AddressMunicipality]
    ([AddressStateId]);
GO

-- Creating foreign key on [DistributorId] in table 'Person_DistributorEmployee'
ALTER TABLE [dbo].[Person_DistributorEmployee]
ADD CONSTRAINT [FK_DistributorDistributorUser]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorDistributorUser'
CREATE INDEX [IX_FK_DistributorDistributorUser]
ON [dbo].[Person_DistributorEmployee]
    ([DistributorId]);
GO

-- Creating foreign key on [AddressMunicipalityId], [AddressStateId] in table 'AddressPostalCode'
ALTER TABLE [dbo].[AddressPostalCode]
ADD CONSTRAINT [FK_AddressMunicipalityAddressPostalCode]
    FOREIGN KEY ([AddressMunicipalityId], [AddressStateId])
    REFERENCES [dbo].[AddressMunicipality]
        ([Id], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressMunicipalityAddressPostalCode'
CREATE INDEX [IX_FK_AddressMunicipalityAddressPostalCode]
ON [dbo].[AddressPostalCode]
    ([AddressMunicipalityId], [AddressStateId]);
GO

-- Creating foreign key on [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId] in table 'AddressColony'
ALTER TABLE [dbo].[AddressColony]
ADD CONSTRAINT [FK_AddressPostalCodeAddressColony]
    FOREIGN KEY ([AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId])
    REFERENCES [dbo].[AddressPostalCode]
        ([Id], [AddressMunicipalityId], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressPostalCodeAddressColony'
CREATE INDEX [IX_FK_AddressPostalCodeAddressColony]
ON [dbo].[AddressColony]
    ([AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId]);
GO

-- Creating foreign key on [AddressColonyId], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId] in table 'Address'
ALTER TABLE [dbo].[Address]
ADD CONSTRAINT [FK_AddressColonyAddress]
    FOREIGN KEY ([AddressColonyId], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId])
    REFERENCES [dbo].[AddressColony]
        ([Id], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressColonyAddress'
CREATE INDEX [IX_FK_AddressColonyAddress]
ON [dbo].[Address]
    ([AddressColonyId], [AddressPostalCodeId], [AddressMunicipalityId], [AddressStateId]);
GO

-- Creating foreign key on [AddressId] in table 'Distributor'
ALTER TABLE [dbo].[Distributor]
ADD CONSTRAINT [FK_AddressDistributor]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Address]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressDistributor'
CREATE INDEX [IX_FK_AddressDistributor]
ON [dbo].[Distributor]
    ([AddressId]);
GO

-- Creating foreign key on [DistributorContactAreaId] in table 'DistributorContact'
ALTER TABLE [dbo].[DistributorContact]
ADD CONSTRAINT [FK_Cat_DistributorContactAreaDistributorContact]
    FOREIGN KEY ([DistributorContactAreaId])
    REFERENCES [dbo].[Cat_DistributorContactArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_DistributorContactAreaDistributorContact'
CREATE INDEX [IX_FK_Cat_DistributorContactAreaDistributorContact]
ON [dbo].[DistributorContact]
    ([DistributorContactAreaId]);
GO

-- Creating foreign key on [DistributorId] in table 'DistributorContact'
ALTER TABLE [dbo].[DistributorContact]
ADD CONSTRAINT [FK_DistributorDistributorContact]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorDistributorContact'
CREATE INDEX [IX_FK_DistributorDistributorContact]
ON [dbo].[DistributorContact]
    ([DistributorId]);
GO

-- Creating foreign key on [AddressId] in table 'DistributorBranch'
ALTER TABLE [dbo].[DistributorBranch]
ADD CONSTRAINT [FK_AddressDistributorBranch]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Address]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressDistributorBranch'
CREATE INDEX [IX_FK_AddressDistributorBranch]
ON [dbo].[DistributorBranch]
    ([AddressId]);
GO

-- Creating foreign key on [Cat_DistributorBranchContactAreaId] in table 'DistributorBranchContact'
ALTER TABLE [dbo].[DistributorBranchContact]
ADD CONSTRAINT [FK_Cat_DistributorBranchContactAreaDistributorBranchContact]
    FOREIGN KEY ([Cat_DistributorBranchContactAreaId])
    REFERENCES [dbo].[Cat_DistributorBranchContactArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_DistributorBranchContactAreaDistributorBranchContact'
CREATE INDEX [IX_FK_Cat_DistributorBranchContactAreaDistributorBranchContact]
ON [dbo].[DistributorBranchContact]
    ([Cat_DistributorBranchContactAreaId]);
GO

-- Creating foreign key on [DistributorBranchId] in table 'DistributorBranchContact'
ALTER TABLE [dbo].[DistributorBranchContact]
ADD CONSTRAINT [FK_DistributorBranchDistributorBranchContact]
    FOREIGN KEY ([DistributorBranchId])
    REFERENCES [dbo].[DistributorBranch]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorBranchDistributorBranchContact'
CREATE INDEX [IX_FK_DistributorBranchDistributorBranchContact]
ON [dbo].[DistributorBranchContact]
    ([DistributorBranchId]);
GO

-- Creating foreign key on [BNAddressId] in table 'Subdistributor'
ALTER TABLE [dbo].[Subdistributor]
ADD CONSTRAINT [FK_AddressSubdistributor]
    FOREIGN KEY ([BNAddressId])
    REFERENCES [dbo].[Address]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressSubdistributor'
CREATE INDEX [IX_FK_AddressSubdistributor]
ON [dbo].[Subdistributor]
    ([BNAddressId]);
GO

-- Creating foreign key on [Cat_ZoneId] in table 'AddressMunicipality'
ALTER TABLE [dbo].[AddressMunicipality]
ADD CONSTRAINT [FK_Cat_ZoneAddressMunicipality]
    FOREIGN KEY ([Cat_ZoneId])
    REFERENCES [dbo].[Cat_Zone]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_ZoneAddressMunicipality'
CREATE INDEX [IX_FK_Cat_ZoneAddressMunicipality]
ON [dbo].[AddressMunicipality]
    ([Cat_ZoneId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'Person_SubdistributorEmployee'
ALTER TABLE [dbo].[Person_SubdistributorEmployee]
ADD CONSTRAINT [FK_SubdistributorSubdistributorEmployee]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorSubdistributorEmployee'
CREATE INDEX [IX_FK_SubdistributorSubdistributorEmployee]
ON [dbo].[Person_SubdistributorEmployee]
    ([SubdistributorId]);
GO

-- Creating foreign key on [AddressId] in table 'AddressesXSubdistributor'
ALTER TABLE [dbo].[AddressesXSubdistributor]
ADD CONSTRAINT [FK_AddressAddressesXDistributor]
    FOREIGN KEY ([AddressId])
    REFERENCES [dbo].[Address]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressAddressesXDistributor'
CREATE INDEX [IX_FK_AddressAddressesXDistributor]
ON [dbo].[AddressesXSubdistributor]
    ([AddressId]);
GO

-- Creating foreign key on [RTV_BayerEmployeeId] in table 'Subdistributor'
ALTER TABLE [dbo].[Subdistributor]
ADD CONSTRAINT [FK_BayerEmployeeSubdistributor]
    FOREIGN KEY ([RTV_BayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeSubdistributor'
CREATE INDEX [IX_FK_BayerEmployeeSubdistributor]
ON [dbo].[Subdistributor]
    ([RTV_BayerEmployeeId]);
GO

-- Creating foreign key on [RTVCreator_BayerEmployeeId] in table 'Subdistributor'
ALTER TABLE [dbo].[Subdistributor]
ADD CONSTRAINT [FK_BayerEmployeeSubdistributor1]
    FOREIGN KEY ([RTVCreator_BayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeSubdistributor1'
CREATE INDEX [IX_FK_BayerEmployeeSubdistributor1]
ON [dbo].[Subdistributor]
    ([RTVCreator_BayerEmployeeId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'SubdistributorContact'
ALTER TABLE [dbo].[SubdistributorContact]
ADD CONSTRAINT [FK_SubdistributorSubdistributorContact]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorSubdistributorContact'
CREATE INDEX [IX_FK_SubdistributorSubdistributorContact]
ON [dbo].[SubdistributorContact]
    ([SubdistributorId]);
GO

-- Creating foreign key on [Cat_SubdistributorContactAreaId] in table 'SubdistributorContact'
ALTER TABLE [dbo].[SubdistributorContact]
ADD CONSTRAINT [FK_Cat_SubdistributorContactAreaSubdistributorContact]
    FOREIGN KEY ([Cat_SubdistributorContactAreaId])
    REFERENCES [dbo].[Cat_SubdistributorContactArea]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_SubdistributorContactAreaSubdistributorContact'
CREATE INDEX [IX_FK_Cat_SubdistributorContactAreaSubdistributorContact]
ON [dbo].[SubdistributorContact]
    ([Cat_SubdistributorContactAreaId]);
GO

-- Creating foreign key on [Cat_CropId] in table 'SubdistributorCropsXMunicipality'
ALTER TABLE [dbo].[SubdistributorCropsXMunicipality]
ADD CONSTRAINT [FK_Cat_CropSubdistributorCropsXMunicipality]
    FOREIGN KEY ([Cat_CropId])
    REFERENCES [dbo].[Cat_Crop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_CropSubdistributorCropsXMunicipality'
CREATE INDEX [IX_FK_Cat_CropSubdistributorCropsXMunicipality]
ON [dbo].[SubdistributorCropsXMunicipality]
    ([Cat_CropId]);
GO

-- Creating foreign key on [AddressMunicipalityId], [AddressMunicipalityAddressStateId] in table 'SubdistributorCropsXMunicipality'
ALTER TABLE [dbo].[SubdistributorCropsXMunicipality]
ADD CONSTRAINT [FK_AddressMunicipalitySubdistributorCropsXMunicipality]
    FOREIGN KEY ([AddressMunicipalityId], [AddressMunicipalityAddressStateId])
    REFERENCES [dbo].[AddressMunicipality]
        ([Id], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressMunicipalitySubdistributorCropsXMunicipality'
CREATE INDEX [IX_FK_AddressMunicipalitySubdistributorCropsXMunicipality]
ON [dbo].[SubdistributorCropsXMunicipality]
    ([AddressMunicipalityId], [AddressMunicipalityAddressStateId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'SubdistributorCropsXMunicipality'
ALTER TABLE [dbo].[SubdistributorCropsXMunicipality]
ADD CONSTRAINT [FK_SubdistributorSubdistributorCropsXMunicipality]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorSubdistributorCropsXMunicipality'
CREATE INDEX [IX_FK_SubdistributorSubdistributorCropsXMunicipality]
ON [dbo].[SubdistributorCropsXMunicipality]
    ([SubdistributorId]);
GO

-- Creating foreign key on [AddressMunicipalityId], [AddressMunicipalityAddressStateId] in table 'DistributorCropsXMunicipality'
ALTER TABLE [dbo].[DistributorCropsXMunicipality]
ADD CONSTRAINT [FK_AddressMunicipalityDistributorCropsXMunicipality]
    FOREIGN KEY ([AddressMunicipalityId], [AddressMunicipalityAddressStateId])
    REFERENCES [dbo].[AddressMunicipality]
        ([Id], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressMunicipalityDistributorCropsXMunicipality'
CREATE INDEX [IX_FK_AddressMunicipalityDistributorCropsXMunicipality]
ON [dbo].[DistributorCropsXMunicipality]
    ([AddressMunicipalityId], [AddressMunicipalityAddressStateId]);
GO

-- Creating foreign key on [Cat_CropId] in table 'DistributorCropsXMunicipality'
ALTER TABLE [dbo].[DistributorCropsXMunicipality]
ADD CONSTRAINT [FK_Cat_CropDistributorCropsXMunicipality]
    FOREIGN KEY ([Cat_CropId])
    REFERENCES [dbo].[Cat_Crop]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_CropDistributorCropsXMunicipality'
CREATE INDEX [IX_FK_Cat_CropDistributorCropsXMunicipality]
ON [dbo].[DistributorCropsXMunicipality]
    ([Cat_CropId]);
GO

-- Creating foreign key on [BayerEmployeeId] in table 'MunicipalitiesXEmployee'
ALTER TABLE [dbo].[MunicipalitiesXEmployee]
ADD CONSTRAINT [FK_BayerEmployeeMunicipalitiesXEmployee]
    FOREIGN KEY ([BayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeMunicipalitiesXEmployee'
CREATE INDEX [IX_FK_BayerEmployeeMunicipalitiesXEmployee]
ON [dbo].[MunicipalitiesXEmployee]
    ([BayerEmployeeId]);
GO

-- Creating foreign key on [AddressMunicipalityId], [AddressMunicipalityAddressStateId] in table 'MunicipalitiesXEmployee'
ALTER TABLE [dbo].[MunicipalitiesXEmployee]
ADD CONSTRAINT [FK_AddressMunicipalityMunicipalitiesXEmployee]
    FOREIGN KEY ([AddressMunicipalityId], [AddressMunicipalityAddressStateId])
    REFERENCES [dbo].[AddressMunicipality]
        ([Id], [AddressStateId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressMunicipalityMunicipalitiesXEmployee'
CREATE INDEX [IX_FK_AddressMunicipalityMunicipalitiesXEmployee]
ON [dbo].[MunicipalitiesXEmployee]
    ([AddressMunicipalityId], [AddressMunicipalityAddressStateId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'SubdistributorCommercialName'
ALTER TABLE [dbo].[SubdistributorCommercialName]
ADD CONSTRAINT [FK_SubdistributorSubdistributorCommercialName]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorSubdistributorCommercialName'
CREATE INDEX [IX_FK_SubdistributorSubdistributorCommercialName]
ON [dbo].[SubdistributorCommercialName]
    ([SubdistributorId]);
GO

-- Creating foreign key on [DistributorId] in table 'DistributorCropsXMunicipality'
ALTER TABLE [dbo].[DistributorCropsXMunicipality]
ADD CONSTRAINT [FK_DistributorDistributorCropsXMunicipality]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorDistributorCropsXMunicipality'
CREATE INDEX [IX_FK_DistributorDistributorCropsXMunicipality]
ON [dbo].[DistributorCropsXMunicipality]
    ([DistributorId]);
GO

-- Creating foreign key on [DistributorId] in table 'DistributorBranch'
ALTER TABLE [dbo].[DistributorBranch]
ADD CONSTRAINT [FK_DistributorDistributorBranch]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorDistributorBranch'
CREATE INDEX [IX_FK_DistributorDistributorBranch]
ON [dbo].[DistributorBranch]
    ([DistributorId]);
GO

-- Creating foreign key on [DistributorId] in table 'ContractDistributor'
ALTER TABLE [dbo].[ContractDistributor]
ADD CONSTRAINT [FK_DistributorContractDistributor]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorContractDistributor'
CREATE INDEX [IX_FK_DistributorContractDistributor]
ON [dbo].[ContractDistributor]
    ([DistributorId]);
GO

-- Creating foreign key on [GRVBayerEmployeeId] in table 'ContractDistributor'
ALTER TABLE [dbo].[ContractDistributor]
ADD CONSTRAINT [FK_BayerEmployeeContractDistributor]
    FOREIGN KEY ([GRVBayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeContractDistributor'
CREATE INDEX [IX_FK_BayerEmployeeContractDistributor]
ON [dbo].[ContractDistributor]
    ([GRVBayerEmployeeId]);
GO

-- Creating foreign key on [RTVBayerEmployeeId] in table 'ContractDistributor'
ALTER TABLE [dbo].[ContractDistributor]
ADD CONSTRAINT [FK_BayerEmployeeContractDistributor1]
    FOREIGN KEY ([RTVBayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeContractDistributor1'
CREATE INDEX [IX_FK_BayerEmployeeContractDistributor1]
ON [dbo].[ContractDistributor]
    ([RTVBayerEmployeeId]);
GO

-- Creating foreign key on [ContractDistributorStatusId] in table 'ContractDistributor'
ALTER TABLE [dbo].[ContractDistributor]
ADD CONSTRAINT [FK_Cat_ContractDistributorStatusContractDistributor]
    FOREIGN KEY ([ContractDistributorStatusId])
    REFERENCES [dbo].[Cat_ContractDistributorStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_ContractDistributorStatusContractDistributor'
CREATE INDEX [IX_FK_Cat_ContractDistributorStatusContractDistributor]
ON [dbo].[ContractDistributor]
    ([ContractDistributorStatusId]);
GO

-- Creating foreign key on [CropCategoryId] in table 'Cat_Crop'
ALTER TABLE [dbo].[Cat_Crop]
ADD CONSTRAINT [FK_Cat_CropCategoryCat_Crop]
    FOREIGN KEY ([CropCategoryId])
    REFERENCES [dbo].[Cat_CropCategory]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_CropCategoryCat_Crop'
CREATE INDEX [IX_FK_Cat_CropCategoryCat_Crop]
ON [dbo].[Cat_Crop]
    ([CropCategoryId]);
GO

-- Creating foreign key on [ContractSubdistributorStatusId] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_Cat_ContractSubdistributorStatusContractSubdistributor]
    FOREIGN KEY ([ContractSubdistributorStatusId])
    REFERENCES [dbo].[Cat_ContractSubdistributorStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Cat_ContractSubdistributorStatusContractSubdistributor'
CREATE INDEX [IX_FK_Cat_ContractSubdistributorStatusContractSubdistributor]
ON [dbo].[ContractSubdistributor]
    ([ContractSubdistributorStatusId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_SubdistributorContractSubdistributor]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorContractSubdistributor'
CREATE INDEX [IX_FK_SubdistributorContractSubdistributor]
ON [dbo].[ContractSubdistributor]
    ([SubdistributorId]);
GO

-- Creating foreign key on [GRVBayerEmployeeId] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_BayerEmployeeContractSubdistributor]
    FOREIGN KEY ([GRVBayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeContractSubdistributor'
CREATE INDEX [IX_FK_BayerEmployeeContractSubdistributor]
ON [dbo].[ContractSubdistributor]
    ([GRVBayerEmployeeId]);
GO

-- Creating foreign key on [RTVBayerEmployeeId] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_BayerEmployeeContractSubdistributor1]
    FOREIGN KEY ([RTVBayerEmployeeId])
    REFERENCES [dbo].[Person_BayerEmployee]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BayerEmployeeContractSubdistributor1'
CREATE INDEX [IX_FK_BayerEmployeeContractSubdistributor1]
ON [dbo].[ContractSubdistributor]
    ([RTVBayerEmployeeId]);
GO

-- Creating foreign key on [NewsId] in table 'NewsSection'
ALTER TABLE [dbo].[NewsSection]
ADD CONSTRAINT [FK_NewsNewsSection]
    FOREIGN KEY ([NewsId])
    REFERENCES [dbo].[News]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NewsNewsSection'
CREATE INDEX [IX_FK_NewsNewsSection]
ON [dbo].[NewsSection]
    ([NewsId]);
GO

-- Creating foreign key on [SubdistributorId] in table 'AddressesXSubdistributor'
ALTER TABLE [dbo].[AddressesXSubdistributor]
ADD CONSTRAINT [FK_SubdistributorAddressesXSubdistributor]
    FOREIGN KEY ([SubdistributorId])
    REFERENCES [dbo].[Subdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubdistributorAddressesXSubdistributor'
CREATE INDEX [IX_FK_SubdistributorAddressesXSubdistributor]
ON [dbo].[AddressesXSubdistributor]
    ([SubdistributorId]);
GO

-- Creating foreign key on [ContractSubdistributorId] in table 'DistributorPurchasesXContractSubdistributor'
ALTER TABLE [dbo].[DistributorPurchasesXContractSubdistributor]
ADD CONSTRAINT [FK_ContractSubdistributorDistributorPurchasesXContractSubdistributor]
    FOREIGN KEY ([ContractSubdistributorId])
    REFERENCES [dbo].[ContractSubdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractSubdistributorDistributorPurchasesXContractSubdistributor'
CREATE INDEX [IX_FK_ContractSubdistributorDistributorPurchasesXContractSubdistributor]
ON [dbo].[DistributorPurchasesXContractSubdistributor]
    ([ContractSubdistributorId]);
GO

-- Creating foreign key on [DistributorId] in table 'DistributorPurchasesXContractSubdistributor'
ALTER TABLE [dbo].[DistributorPurchasesXContractSubdistributor]
ADD CONSTRAINT [FK_DistributorDistributorPurchasesXContractSubdistributor]
    FOREIGN KEY ([DistributorId])
    REFERENCES [dbo].[Distributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DistributorDistributorPurchasesXContractSubdistributor'
CREATE INDEX [IX_FK_DistributorDistributorPurchasesXContractSubdistributor]
ON [dbo].[DistributorPurchasesXContractSubdistributor]
    ([DistributorId]);
GO

-- Creating foreign key on [CurrentContract_Id] in table 'Distributor'
ALTER TABLE [dbo].[Distributor]
ADD CONSTRAINT [FK_ContractDistributorDistributor]
    FOREIGN KEY ([CurrentContract_Id])
    REFERENCES [dbo].[ContractDistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractDistributorDistributor'
CREATE INDEX [IX_FK_ContractDistributorDistributor]
ON [dbo].[Distributor]
    ([CurrentContract_Id]);
GO

-- Creating foreign key on [CurrentContract_Id] in table 'Subdistributor'
ALTER TABLE [dbo].[Subdistributor]
ADD CONSTRAINT [FK_ContractSubdistributorSubdistributor]
    FOREIGN KEY ([CurrentContract_Id])
    REFERENCES [dbo].[ContractSubdistributor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractSubdistributorSubdistributor'
CREATE INDEX [IX_FK_ContractSubdistributorSubdistributor]
ON [dbo].[Subdistributor]
    ([CurrentContract_Id]);
GO

-- Creating foreign key on [SubdistributorDiscountCoupon_Id] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_ContractSubdistributorSubdistributorDiscountCoupon]
    FOREIGN KEY ([SubdistributorDiscountCoupon_Id])
    REFERENCES [dbo].[SubdistributorDiscountCoupon]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractSubdistributorSubdistributorDiscountCoupon'
CREATE INDEX [IX_FK_ContractSubdistributorSubdistributorDiscountCoupon]
ON [dbo].[ContractSubdistributor]
    ([SubdistributorDiscountCoupon_Id]);
GO

-- Creating foreign key on [SubdistributorPromotionCoupon_Id] in table 'ContractSubdistributor'
ALTER TABLE [dbo].[ContractSubdistributor]
ADD CONSTRAINT [FK_ContractSubdistributorSubdistributorPromotionCoupon]
    FOREIGN KEY ([SubdistributorPromotionCoupon_Id])
    REFERENCES [dbo].[SubdistributorPromotionCoupon]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractSubdistributorSubdistributorPromotionCoupon'
CREATE INDEX [IX_FK_ContractSubdistributorSubdistributorPromotionCoupon]
ON [dbo].[ContractSubdistributor]
    ([SubdistributorPromotionCoupon_Id]);
GO

-- Creating foreign key on [Id] in table 'Person_DistributorEmployee'
ALTER TABLE [dbo].[Person_DistributorEmployee]
ADD CONSTRAINT [FK_DistributorEmployee_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Person_SubdistributorEmployee'
ALTER TABLE [dbo].[Person_SubdistributorEmployee]
ADD CONSTRAINT [FK_SubdistributorEmployee_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Person_BayerEmployee'
ALTER TABLE [dbo].[Person_BayerEmployee]
ADD CONSTRAINT [FK_BayerEmployee_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------