USE [master]
GO
/****** Object:  Database [ApiH2H]    Script Date: 20/03/2020 11:52:12 ******/
CREATE DATABASE [ApiH2H]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ApiH2H', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ApiH2H.mdf' , SIZE = 139264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ApiH2H_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ApiH2H_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ApiH2H] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ApiH2H].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ApiH2H] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ApiH2H] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ApiH2H] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ApiH2H] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ApiH2H] SET ARITHABORT OFF 
GO
ALTER DATABASE [ApiH2H] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ApiH2H] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ApiH2H] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ApiH2H] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ApiH2H] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ApiH2H] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ApiH2H] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ApiH2H] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ApiH2H] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ApiH2H] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ApiH2H] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ApiH2H] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ApiH2H] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ApiH2H] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ApiH2H] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ApiH2H] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ApiH2H] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ApiH2H] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ApiH2H] SET  MULTI_USER 
GO
ALTER DATABASE [ApiH2H] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ApiH2H] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ApiH2H] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ApiH2H] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ApiH2H] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ApiH2H] SET QUERY_STORE = OFF
GO
USE [ApiH2H]
GO
/****** Object:  User [WIN-BUJDEBV9J1A\DefaultAccount]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [WIN-BUJDEBV9J1A\DefaultAccount] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [WIN-BUJDEBV9J1A\Administrator]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [WIN-BUJDEBV9J1A\Administrator] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [IIS APPPOOL\DefaultAppPool]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [IIS APPPOOL\DefaultAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [IIS APPPOOL\apitest]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [IIS APPPOOL\apitest] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [IIS APPPOOL\api]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [IIS APPPOOL\api] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [api]    Script Date: 20/03/2020 11:52:12 ******/
CREATE USER [api] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [WIN-BUJDEBV9J1A\DefaultAccount]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\DefaultAppPool]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\apitest]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\api]
GO
ALTER ROLE [db_owner] ADD MEMBER [api]
GO
/****** Object:  Table [dbo].[Bulletins]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bulletins](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[namesId] [int] NOT NULL,
	[NumeroContoCorrente] [nvarchar](max) NOT NULL,
	[IntestatoA] [nvarchar](max) NOT NULL,
	[FormatoStampa] [nvarchar](max) NULL,
	[Template] [nvarchar](max) NULL,
	[AdditionalInfo] [nvarchar](max) NULL,
	[IBAN] [nvarchar](max) NULL,
	[CodiceCliente] [nvarchar](max) NOT NULL,
	[ImportoEuro] [decimal](10, 2) NOT NULL,
	[EseguitoDaNominativo] [nvarchar](max) NOT NULL,
	[EseguitoDaIndirizzo] [nvarchar](max) NOT NULL,
	[EseguitoDaCAP] [nvarchar](max) NOT NULL,
	[EseguitoDaLocalita] [nvarchar](max) NOT NULL,
	[Causale] [nvarchar](max) NOT NULL,
	[BulletinType] [int] NOT NULL,
	[namesListsId] [int] NOT NULL,
 CONSTRAINT [PK_Bulletins] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Names]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Names](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[businessName] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[surname] [nvarchar](50) NULL,
	[complementNames] [nvarchar](max) NULL,
	[complementAddress] [nvarchar](max) NULL,
	[dug] [nvarchar](10) NULL,
	[address] [nvarchar](50) NOT NULL,
	[houseNumber] [nvarchar](50) NULL,
	[cap] [nvarchar](10) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[province] [nvarchar](10) NOT NULL,
	[state] [nvarchar](50) NOT NULL,
	[insertDate] [datetime] NOT NULL,
	[currentState] [int] NOT NULL,
	[valid] [bit] NOT NULL,
	[operationId] [int] NOT NULL,
	[requestId] [nvarchar](max) NULL,
	[orderId] [nvarchar](max) NULL,
	[codice] [nvarchar](max) NULL,
	[price] [decimal](10, 2) NULL,
	[vatPrice] [decimal](10, 2) NULL,
	[totalPrice] [decimal](10, 2) NULL,
	[attachedFile] [varbinary](max) NULL,
	[fileName] [nvarchar](max) NULL,
	[presaInCaricoDate] [datetime] NULL,
	[consegnatoDate] [datetime] NULL,
	[stato] [nvarchar](550) NULL,
	[guidUser] [nvarchar](550) NULL,
	[tipoStampa] [bit] NULL,
	[fronteRetro] [bit] NULL,
	[ricevutaRitorno] [bit] NULL,
	[locked] [bit] NOT NULL,
	[reSendGuid] [uniqueidentifier] NULL,
	[fiscalCode] [nvarchar](50) NULL,
	[mobile] [nvarchar](50) NULL,
 CONSTRAINT [PK_Names] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[operationFeatures]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[operationFeatures](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[operationId] [int] NOT NULL,
	[featureType] [nvarchar](550) NOT NULL,
	[featureValue] [nvarchar](550) NOT NULL,
 CONSTRAINT [PK_operationFeatures] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operations]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](450) NOT NULL,
	[userId] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[operationType] [int] NOT NULL,
	[demoOperation] [bit] NOT NULL,
	[complete] [bit] NOT NULL,
	[userParentId] [int] NULL,
	[areaTestOperation] [bit] NOT NULL,
 CONSTRAINT [PK_Operations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Property]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](550) NULL,
	[description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Senders]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Senders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](350) NULL,
	[surname] [nvarchar](350) NULL,
	[businessName] [nvarchar](max) NULL,
	[dug] [nvarchar](10) NOT NULL,
	[address] [nvarchar](50) NOT NULL,
	[houseNumber] [nvarchar](5) NULL,
	[cap] [nvarchar](5) NOT NULL,
	[city] [nvarchar](350) NOT NULL,
	[province] [nvarchar](2) NOT NULL,
	[state] [nvarchar](50) NOT NULL,
	[operationId] [int] NOT NULL,
 CONSTRAINT [PK_SendersUsers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20/03/2020 11:52:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usernamePoste] [nvarchar](350) NULL,
	[pwdPoste] [nvarchar](350) NULL,
	[porpertyId] [int] NOT NULL,
	[name] [nvarchar](450) NOT NULL,
	[lastName] [nvarchar](450) NOT NULL,
	[userType] [int] NOT NULL,
	[email] [nvarchar](250) NOT NULL,
	[guidUser] [uniqueidentifier] NOT NULL,
	[businessName] [nvarchar](550) NOT NULL,
	[baseUrl] [nvarchar](max) NOT NULL,
	[pwd] [nvarchar](250) NOT NULL,
	[address] [nvarchar](500) NOT NULL,
	[cap] [nvarchar](5) NOT NULL,
	[city] [nvarchar](250) NOT NULL,
	[province] [nvarchar](50) NOT NULL,
	[mobile] [nvarchar](50) NOT NULL,
	[demoUser] [bit] NOT NULL,
	[parentId] [int] NOT NULL,
	[sendersId] [nvarchar](max) NULL,
	[usernamePosteAreaTest] [nvarchar](350) NULL,
	[pwdPosteAreaTest] [nvarchar](350) NULL,
	[areaTestUser] [bit] NOT NULL,
	[molCol] [bit] NULL,
	[CodiceContrattoMOL] [nvarchar](250) NULL,
	[CodiceContrattoCOL] [nvarchar](250) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Names] ON 

INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19482, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:27.347' AS DateTime), 0, 1, 872, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 0x, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:26.227' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19483, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:41.310' AS DateTime), 0, 1, 873, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 0x, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:40.937' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19489, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:41.310' AS DateTime), 0, 1, 873, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:40.937' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19490, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:41.310' AS DateTime), 0, 1, 873, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:40.937' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19491, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:41.310' AS DateTime), 0, 1, 873, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:40.937' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
INSERT [dbo].[Names] ([id], [businessName], [name], [surname], [complementNames], [complementAddress], [dug], [address], [houseNumber], [cap], [city], [province], [state], [insertDate], [currentState], [valid], [operationId], [requestId], [orderId], [codice], [price], [vatPrice], [totalPrice], [attachedFile], [fileName], [presaInCaricoDate], [consegnatoDate], [stato], [guidUser], [tipoStampa], [fronteRetro], [ricevutaRitorno], [locked], [reSendGuid], [fiscalCode], [mobile]) VALUES (19492, N'', N'mario', N'rossi', N'', N'', N'', N'via toledo,30', N'', N'80132', N'napoli', N'Na', N'italia', CAST(N'2020-03-11T16:14:41.310' AS DateTime), 0, 1, 873, NULL, NULL, NULL, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), NULL, N'C:\inetpub\wwwroot\website\Upload\Users\1\637195400471341779/1.pdf', CAST(N'2020-03-11T16:16:40.937' AS DateTime), NULL, NULL, NULL, 1, 0, 0, 0, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Names] OFF
SET IDENTITY_INSERT [dbo].[operationFeatures] ON 

INSERT [dbo].[operationFeatures] ([id], [operationId], [featureType], [featureValue]) VALUES (2558, 872, N'tipo stampa', N'biancoNero')
INSERT [dbo].[operationFeatures] ([id], [operationId], [featureType], [featureValue]) VALUES (2559, 872, N'fronte Retro', N'fronte')
INSERT [dbo].[operationFeatures] ([id], [operationId], [featureType], [featureValue]) VALUES (2560, 873, N'tipo stampa', N'biancoNero')
INSERT [dbo].[operationFeatures] ([id], [operationId], [featureType], [featureValue]) VALUES (2561, 873, N'fronte Retro', N'fronte')
SET IDENTITY_INSERT [dbo].[operationFeatures] OFF
SET IDENTITY_INSERT [dbo].[Operations] ON 

INSERT [dbo].[Operations] ([id], [name], [userId], [date], [operationType], [demoOperation], [complete], [userParentId], [areaTestOperation]) VALUES (872, N' Operazione del 11/03/2020', 1, CAST(N'2020-03-11T16:14:27.033' AS DateTime), 2, 0, 1, 0, 1)
INSERT [dbo].[Operations] ([id], [name], [userId], [date], [operationType], [demoOperation], [complete], [userParentId], [areaTestOperation]) VALUES (873, N' Operazione del 11/03/2020', 1, CAST(N'2020-03-11T16:14:41.297' AS DateTime), 2, 0, 1, 0, 1)
SET IDENTITY_INSERT [dbo].[Operations] OFF
SET IDENTITY_INSERT [dbo].[Property] ON 

INSERT [dbo].[Property] ([id], [name], [description]) VALUES (1, N'EWT', N'')
SET IDENTITY_INSERT [dbo].[Property] OFF
SET IDENTITY_INSERT [dbo].[Senders] ON 

INSERT [dbo].[Senders] ([id], [name], [surname], [businessName], [dug], [address], [houseNumber], [cap], [city], [province], [state], [operationId]) VALUES (2, N'', N'', N'ewt', N'via', N'toledo', N'11', N'80132', N'napoli', N'na', N'italia', 872)
INSERT [dbo].[Senders] ([id], [name], [surname], [businessName], [dug], [address], [houseNumber], [cap], [city], [province], [state], [operationId]) VALUES (304, N'', N'', N'ewt', N'via', N'toledo', N'11', N'80132', N'napoli', N'na', N'italia', 873)
SET IDENTITY_INSERT [dbo].[Senders] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [usernamePoste], [pwdPoste], [porpertyId], [name], [lastName], [userType], [email], [guidUser], [businessName], [baseUrl], [pwd], [address], [cap], [city], [province], [mobile], [demoUser], [parentId], [sendersId], [usernamePosteAreaTest], [pwdPosteAreaTest], [areaTestUser], [molCol], [CodiceContrattoMOL], [CodiceContrattoCOL]) VALUES (1, N'HH800521', N'Gc998086', 1, N'administrator', N'', 1, N'demo@master.it', N'a83b4c97-4b83-44f0-a233-4aaf30816e70', N'', N'/public/Ewt/', N'master', N'toledo', N'80132', N'napoli', N'Napoli    ', N'', 0, 0, NULL, N'H2HST373', N'Cestg373', 1, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [IX_Names]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Names] ON [dbo].[Names]
(
	[insertDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Names_1]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Names_1] ON [dbo].[Names]
(
	[locked] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Names_2]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Names_2] ON [dbo].[Names]
(
	[operationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Names_3]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Names_3] ON [dbo].[Names]
(
	[valid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Names_4]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Names_4] ON [dbo].[Names]
(
	[currentState] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_operationFeatures]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_operationFeatures] ON [dbo].[operationFeatures]
(
	[operationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations] ON [dbo].[Operations]
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations_1]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations_1] ON [dbo].[Operations]
(
	[date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations_2]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations_2] ON [dbo].[Operations]
(
	[operationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations_3]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations_3] ON [dbo].[Operations]
(
	[demoOperation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations_4]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations_4] ON [dbo].[Operations]
(
	[complete] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operations_5]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_Operations_5] ON [dbo].[Operations]
(
	[areaTestOperation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SendersUsers]    Script Date: 20/03/2020 11:52:12 ******/
CREATE NONCLUSTERED INDEX [IX_SendersUsers] ON [dbo].[Senders]
(
	[operationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Names] ADD  CONSTRAINT [DF_Table_1_StatoCorrente]  DEFAULT ((1)) FOR [currentState]
GO
ALTER TABLE [dbo].[Names] ADD  CONSTRAINT [DF_Table_1_Valido]  DEFAULT ((0)) FOR [valid]
GO
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_demoOperation]  DEFAULT ((0)) FOR [demoOperation]
GO
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_complete]  DEFAULT ((0)) FOR [complete]
GO
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_areaTestOperation]  DEFAULT ((0)) FOR [areaTestOperation]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_giudUser]  DEFAULT (newid()) FOR [guidUser]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_parentId]  DEFAULT ((0)) FOR [parentId]
GO
ALTER TABLE [dbo].[Names]  WITH CHECK ADD  CONSTRAINT [FK_Names_Operations] FOREIGN KEY([operationId])
REFERENCES [dbo].[Operations] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Names] CHECK CONSTRAINT [FK_Names_Operations]
GO
ALTER TABLE [dbo].[operationFeatures]  WITH CHECK ADD  CONSTRAINT [FK_operationFeatures_Operations] FOREIGN KEY([operationId])
REFERENCES [dbo].[Operations] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[operationFeatures] CHECK CONSTRAINT [FK_operationFeatures_Operations]
GO
ALTER TABLE [dbo].[Operations]  WITH CHECK ADD  CONSTRAINT [FK_Operations_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Operations] CHECK CONSTRAINT [FK_Operations_Users]
GO
ALTER TABLE [dbo].[Senders]  WITH CHECK ADD  CONSTRAINT [FK_Senders_Operations] FOREIGN KEY([operationId])
REFERENCES [dbo].[Operations] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Senders] CHECK CONSTRAINT [FK_Senders_Operations]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Property] FOREIGN KEY([porpertyId])
REFERENCES [dbo].[Property] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Property]
GO
USE [master]
GO
ALTER DATABASE [ApiH2H] SET  READ_WRITE 
GO
