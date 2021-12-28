SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Model klasse Adres business rules vormen geen probleem
CREATE TABLE [dbo].[Adres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Straatnaam] [varchar](150) NOT NULL,
	[Huisnummer] [varchar](50) NOT NULL,
	[Postcode] [varchar](50) NOT NULL,
	[Plaatsnaam] [varchar](150) NOT NULL,
	[Provincie] [varchar](150) NOT NULL,
	[Land] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Adres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------------------------------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Model klasse Tankkaart business rules gecontroleerd en vormen geen probleem, 
-- Uitzonderingen: Pincode moet daar 4 karakters lang zijn
CREATE TABLE [dbo].[Tankkaart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kaartnummer] [varchar](60) NOT NULL,
	[Vervaldatum] [date] NOT NULL,
	[Pincode] [varchar](50) NULL,
	-- [Brandstof] [varchar](50) NULL, --<< deze valt weg en krijgt tussentabel
 CONSTRAINT [PK_Tankkaart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------------------------------
/****** Object:  Table [dbo].[Voertuig]    Script Date: 22/10/2021 19:47:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Model klasse Voertuig business rules gecontroleerd en vormen geen probleem, 
-- Uitzonderingen: model, nummerplaat max 20, chassisnr fixed 17, kleur max 40, deuren value max 20
CREATE TABLE [dbo].[Voertuig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Merk] [varchar](50) NOT NULL,
	[Model] [varchar](50) NOT NULL,
	[Nummerplaat] [varchar](50) NOT NULL,
	[Chasisnummer] [varchar](250) NOT NULL,
	[Brandstof] [varchar](50) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Kleur] [varchar](50) NULL,
	[AantalDeuren] [varchar](50) NULL
 CONSTRAINT [PK_Voertuig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------------------------
/****** Object:  Table [dbo].[TankkaartBrandstof]    Script Date: 22/10/2021 19:47:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Model OK (enum is restrictie)
CREATE TABLE [dbo].[TankkaartBrandstof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TankkaartId] [int] NOT NULL,
	[Brandstof] [varchar](75) NOT NULL
 CONSTRAINT [PK_TankkaartBrandstof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TankkaartBrandstof]  WITH CHECK ADD  CONSTRAINT [FK_TankkaartBrandstof_Tankkaart] FOREIGN KEY([TankkaartId])
REFERENCES [dbo].[Tankkaart] ([Id])
GO

ALTER TABLE [dbo].[TankkaartBrandstof] CHECK CONSTRAINT [FK_TankkaartBrandstof_Tankkaart]
GO
--------------------------------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Model klasse Bestuurder business rules gecontroleerd en vormen geen probleem, 
-- Uitzonderingen: voornaam, naam max geen digits bevatten, min 2 chars
CREATE TABLE [dbo].[Bestuurder](
	[Id] [int] IDENTITY(1,1) NOT NULL,  --<< auto increment bij alle tabellen toegevoegd
	[Naam] [varchar](75) NOT NULL,
	[Voornaam] [varchar](75) NOT NULL,
	[Geboortedatum] [date] NOT NULL,
	[AdresId] [int] NULL,
	[Rijksregisternummer] [varchar](11) NOT NULL, --<< veld uit klasse nog
	[Rijbewijssoort] [varchar](30) NOT NULL, --<< veld uit klasse nog
	[VoertuigId] [int] NULL, --<< in de plaats van VoertuigBestuurder tabel
	[TankkaartId] [int] NULL --<< in de plaats van TankkaartBestuurder tabel
 CONSTRAINT [PK_Bestuurder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Adres] FOREIGN KEY([AdresId])
REFERENCES [dbo].[Adres] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Adres]
GO

-- extra FK constraint voor voertuigid
ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Voertuig] FOREIGN KEY([VoertuigId])
REFERENCES [dbo].[Voertuig] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Voertuig]
GO

-- extra FK constraint voor tankkaartid
ALTER TABLE [dbo].[Bestuurder]  WITH CHECK ADD  CONSTRAINT [FK_Bestuurder_Tankkaart] FOREIGN KEY([TankkaartId])
REFERENCES [dbo].[Tankkaart] ([Id])
GO

ALTER TABLE [dbo].[Bestuurder] CHECK CONSTRAINT [FK_Bestuurder_Tankkaart]
GO