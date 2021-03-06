USE [MCMSDB]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Application] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AviContentd]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AviContentd](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MID] [int] NOT NULL,
	[AVIContent] [varbinary](max) NOT NULL,
	[ContentType] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AviContentd] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contentd]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contentd](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MID] [int] NOT NULL,
	[RowID] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[AVIContent] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CssID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Contentd] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contentm]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contentm](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LinksID] [int] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[SecondName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Contentm] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CSS]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CSS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](max) NOT NULL,
	[StyleSheetName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.CSS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Links]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Links](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Parent] [int] NOT NULL,
	[URL] [nvarchar](max) NOT NULL,
	[IMG_Path] [nvarchar](max) NULL,
	[IsParent] [bit] NOT NULL,
	[Frame] [nvarchar](max) NULL,
	[SQID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Links] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_roles]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_roles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.mcms_roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_SubcriptionCheck]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_SubcriptionCheck](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[ExpireDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[SelectUnsbStatus] [nvarchar](max) NULL,
	[SubscribeDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.mcms_SubcriptionCheck] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_userclaims]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_userclaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.mcms_userclaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_userlogins]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_userlogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.mcms_userlogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_userroles]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_userroles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.mcms_userroles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mcms_users]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mcms_users](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.mcms_users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[row_details]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[row_details](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](max) NULL,
	[ControllerName] [nvarchar](max) NULL,
	[ActionName] [nvarchar](max) NULL,
	[IsAccess] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.row_details] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SOAP_Log]    Script Date: 11/5/2016 12:31:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SOAP_Log](
	[idn] [int] IDENTITY(1,1) NOT NULL,
	[request_type] [nvarchar](max) NULL,
	[referance_code] [nvarchar](max) NULL,
	[msisdn] [nvarchar](max) NULL,
	[spid] [nvarchar](max) NULL,
	[productid] [nvarchar](max) NULL,
	[opercode] [nvarchar](max) NULL,
	[isautoextend] [nvarchar](max) NULL,
	[channelid] [nvarchar](max) NULL,
	[timestamp] [nvarchar](max) NULL,
	[request_xml] [nvarchar](max) NULL,
	[response_xml] [nvarchar](max) NULL,
	[result_code] [nvarchar](max) NULL,
	[resulttime] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[ContentID] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SOAP_Log] PRIMARY KEY CLUSTERED 
(
	[idn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[mcms_userclaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.mcms_userclaims_dbo.mcms_users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[mcms_users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[mcms_userclaims] CHECK CONSTRAINT [FK_dbo.mcms_userclaims_dbo.mcms_users_UserId]
GO
ALTER TABLE [dbo].[mcms_userlogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.mcms_userlogins_dbo.mcms_users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[mcms_users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[mcms_userlogins] CHECK CONSTRAINT [FK_dbo.mcms_userlogins_dbo.mcms_users_UserId]
GO
ALTER TABLE [dbo].[mcms_userroles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.mcms_userroles_dbo.mcms_roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[mcms_roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[mcms_userroles] CHECK CONSTRAINT [FK_dbo.mcms_userroles_dbo.mcms_roles_RoleId]
GO
ALTER TABLE [dbo].[mcms_userroles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.mcms_userroles_dbo.mcms_users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[mcms_users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[mcms_userroles] CHECK CONSTRAINT [FK_dbo.mcms_userroles_dbo.mcms_users_UserId]
GO
