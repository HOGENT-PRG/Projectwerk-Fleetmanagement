/****** Object:  Table [dbo].[Tankkaart]    Script Date: 01/01/2022 17:38:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tankkaart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kaartnummer] [varchar](60) NOT NULL,
	[Vervaldatum] [date] NOT NULL,
	[Pincode] [varchar](50) NULL,
	[IsGeblokkeerd] [bit] NOT NULL,
 CONSTRAINT [PK_Tankkaart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tankkaart] ADD  DEFAULT ((0)) FOR [IsGeblokkeerd]
GO


