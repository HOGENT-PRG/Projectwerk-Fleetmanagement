/****** Object:  Table [dbo].[Tankkaart]    Script Date: 22/10/2021 19:47:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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


