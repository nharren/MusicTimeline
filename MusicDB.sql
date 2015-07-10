CREATE DATABASE [Music]
GO

USE [Music]
GO

CREATE TABLE [dbo].[Album](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](255) NOT NULL
	CONSTRAINT [PK_Album] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[CatalogNumber](
	[ID] [smallint] NOT NULL,
	[Number] [nvarchar](15) NOT NULL,
	[CompositionCatalogID] [smallint] NOT NULL,
	[CompositionCollectionID] [smallint],
	[CompositionID] [int]
	CONSTRAINT [PK_CatalogNumber] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Composer](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Dates] [nvarchar](50) NOT NULL,
	[BirthLocationID] [int],
	[DeathLocationID] [int],
	[Biography] [nvarchar](max),
	[IsPopular] [bit] NOT NULL
	CONSTRAINT [PK_Composer] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[ComposerEra](
	[EraID] [smallint] NOT NULL,
	[ComposerID] [smallint] NOT NULL,
    CONSTRAINT [PK_ComposerEra] PRIMARY KEY ([EraID], [ComposerID])
)
GO

CREATE TABLE [dbo].[ComposerImage](
	[ID] [smallint] NOT NULL,
	[ComposerID] [smallint] NOT NULL,
	[Image] [varbinary](max) NOT NULL,
    CONSTRAINT [PK_ComposerImage] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[ComposerInfluence](
	[ComposerID] [smallint] NOT NULL,
	[InfluenceID] [smallint] NOT NULL,
    CONSTRAINT [PK_ComposerInfluence] PRIMARY KEY ([ComposerID], [InfluenceID])
)
GO

CREATE TABLE [dbo].[ComposerLink](
	[ID] [smallint] NOT NULL,
	[URL] [nvarchar](255) NOT NULL,
	[ComposerID] [smallint] NOT NULL,
    CONSTRAINT [PK_ComposerLink] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[ComposerNationality](
	[ComposerID] [smallint] NOT NULL,
	[NationalityID] [smallint] NOT NULL,
    CONSTRAINT [PK_ComposerNationality] PRIMARY KEY ([ComposerID], [NationalityID])
)
GO

CREATE TABLE [dbo].[Composition](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Dates] [nvarchar](50) NOT NULL,
	[Nickname] [nvarchar](50),
	[IsPopular] [bit] NOT NULL,
	[CompositionCollectionID] [smallint],
    CONSTRAINT [PK_Composition] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[CompositionCatalog](
	[ID] [smallint] NOT NULL,
	[Prefix] [nvarchar](10) NOT NULL,
	[ComposerID] [smallint] NOT NULL,
    CONSTRAINT [PK_CompositionCatalog] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[CompositionCollection](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsPopular] [bit] NOT NULL,
    CONSTRAINT [PK_CompositionCollection] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[CompositionCollectionComposer](
	[ComposerID] [smallint] NOT NULL,
	[CompositionCollectionID] [smallint] NOT NULL,
    CONSTRAINT [PK_CompositionCollectionComposer] PRIMARY KEY ([ComposerID], [CompositionCollectionID])
)
GO

CREATE TABLE [dbo].[CompositionComposer](
	[ComposerID] [smallint] NOT NULL,
	[CompositionID] [int] NOT NULL,
    CONSTRAINT [PK_CompositionComposer] PRIMARY KEY ([ComposerID], [CompositionID])
)
GO

CREATE TABLE [dbo].[Era](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](12) NOT NULL,
	[Dates] [nvarchar](9) NOT NULL,
    CONSTRAINT [PK_Era] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Location](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Movement](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Number] [smallint] NOT NULL,
	[CompositionID] [int] NOT NULL,
	[IsPopular] [bit] NOT NULL,
    CONSTRAINT [PK_Movement] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Nationality](
	[ID] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Nationality] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Performer](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Performer] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[Recording](
	[ID] [int] NOT NULL,
	[Dates] [nvarchar](255) NOT NULL,
	[AlbumID] [smallint],
	[TrackNumber] [smallint],
	[CompositionCollectionID] [smallint],
	[CompositionID] [int],
	[MovementID] [int],
    CONSTRAINT [PK_Recording] PRIMARY KEY ([ID])
)
GO

CREATE TABLE [dbo].[RecordingLocation](
	[RecordingID] [int] NOT NULL,
	[LocationID] [int] NOT NULL,
    CONSTRAINT [PK_RecordingLocation] PRIMARY KEY ([RecordingID], [LocationID])
)
GO

CREATE TABLE [dbo].[RecordingPerformer](
	[RecordingID] [int] NOT NULL,
	[PerformerID] [int] NOT NULL,
    CONSTRAINT [PK_RecordingPerformer] PRIMARY KEY ([RecordingID], [PerformerID])
)
GO

INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (1, N'Albéniz, Isaac', N'1860-05-29/1909-05-18', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (2, N'Albinoni, Tomaso', N'1671-06-08/1751-01-17', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (3, N'Arnold, Malcolm', N'1921-10-21/2006-09-23', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (4, N'Bach, Johann Christian', N'1735-09-05/1782-01-01', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (5, N'Bach, Johann Sebastian', N'1685-03-31/1750-07-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (6, N'Barber, Samuel', N'1910-03-09/1981-01-23', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (7, N'Bartók, Béla', N'1881-03-25/1945-09-26', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (8, N'Beethoven, Ludwig van', N'1770-12-17/1827-03-26', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (9, N'Bellini, Vincenzo', N'1801-11-03/1835-09-23', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (10, N'Berlioz, Hector', N'1803-12-11/1869-03-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (11, N'Bernstein, Leonard', N'1918-08-25/1990-10-14', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (12, N'Bizet, Georges', N'1838-10-25/1875-06-03', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (13, N'Bloch, Ernest', N'1885-07-24/1959-07-15', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (14, N'Boccherini, Luigi', N'1743-02-19/1805-05-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (15, N'Borodin, Alexander', N'1833-11-12/1887-02-27', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (16, N'Brahms, Johannes', N'1833-05-07/1897-04-03', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (17, N'Britten, Benjamin', N'1913-11-22/1976-12-04', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (18, N'Bruch, Max', N'1838-01-06/1920-10-02', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (19, N'Bruckner, Anton', N'1824-09-04/1896-10-11', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (20, N'Byrd, William', N'[1539,1540,1543]/1623-07-14', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (21, N'Chopin, Frédéric', N'[1810-02-22,1810-03-01]/1849-10-17', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (22, N'Copland, Aaron', N'1900-11-14/1990-12-02', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (23, N'Corelli, Arcangelo', N'1653-02-17/1713-01-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (24, N'Debussy, Claude', N'1862-08-22/1918-03-25', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (25, N'Delibes, Léo', N'1836-02-21/1891-01-16', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (26, N'Delius, Frederick', N'1862-01-29/1934-06-10', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (27, N'Donizetti, Gaetano', N'1797-11-29/1848-04-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (28, N'Dvořák, Antonín', N'1841-09-08/1904-05-01', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (29, N'Elgar, Edward', N'1857-06-02/1934-02-23', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (30, N'Falla, Manuel de', N'1876-11-23/1946-11-14', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (31, N'Fauré, Gabriel', N'1845-05-12/1924-11-04', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (32, N'Franck, César', N'1822-12-10/1890-11-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (33, N'Gershwin, George', N'1898-09-26/1937-07-11', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (34, N'Glazunov, Alexander', N'1865-08-10/1936-03-21', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (35, N'Gluck, Christoph Willibald', N'1714-07-02/1787-11-15', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (36, N'Gounod, Charles', N'1818-06-17/[1893-10-17,1893-10-18]', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (37, N'Granados, Enrique', N'1867-07-27/1916-03-24', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (38, N'Grieg, Edvard', N'1843-06-15/1907-09-04', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (39, N'Handel, George Frideric', N'1685-02-23/1759-04-14', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (40, N'Haydn, Joseph', N'1732-03-31/1809-05-31', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (41, N'Hindemith, Paul', N'1895-11-16/1963-12-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (42, N'Holst, Gustav', N'1874-09-21/1934-05-25', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (43, N'Hummel, Johann Nepomuk', N'1778-11-14/1837-10-17', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (44, N'Ives, Charles', N'1874-10-20/1954-05-19', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (45, N'Janáček, Leoš', N'1854-07-03/1928-08-12', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (46, N'Khachaturain, Aram', N'1903-06-06/1978-05-01', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (47, N'Korngold, Erich Wolfgang', N'1897-05-29/1957-11-29', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (48, N'Kreisler, Fritz', N'1875-02-02/1962-01-29', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (49, N'Lehár, Franz', N'1870-04-30/1948-10-24', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (50, N'Leoncavallo, Ruggero', N'1857-04-23/1919-08-09', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (51, N'Liszt, Franz', N'1811-10-22/1886-07-31', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (52, N'Lully, Jean-Baptiste', N'1632-11-28/1687-03-22', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (53, N'Mahler, Gustav', N'1860-07-07/1911-05-18', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (54, N'Marais, Marin', N'1656-05-31/1728-08-15', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (55, N'Mascagni, Pietro', N'1863-12-07/1945-08-02', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (56, N'Massenet, Jules', N'1842-05-12/1912-08-13', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (57, N'Mendelssohn, Felix', N'1809-02-03/1847-11-04', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (58, N'Monteverdi, Claudio', N'1567-05-15~/1643-11-29', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (59, N'Mozart, Wolfgang Amadeus', N'1756-01-27/1791-12-05', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (60, N'Mussorgsky, Modest', N'1839-03-21/1881-03-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (61, N'Nielsen, Carl', N'1865-06-09/1931-10-03', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (62, N'Offenbach, Jacques', N'1819-06-20/1880-10-05', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (63, N'Pachelbel, Johann', N'1653-09-01~/1706-03-09~', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (64, N'Paganini, Niccolò', N'1782-10-27/1840-05-27', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (65, N'Ponce, Manuel', N'1882-12-08/1948-04-24', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (66, N'Poulenc, Francis', N'1899-01-07/1963-01-30', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (67, N'Prokofiev, Sergei', N'1891-04-27/1953-03-05', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (68, N'Puccini, Giacomo', N'1858-12-22/1924-11-29', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (69, N'Purcell, Henry', N'(1659-09-10)?/1695-11-21', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (70, N'Pärt, Arvo', N'1935-09-11/open', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (71, N'Rachmaninoff, Sergei', N'1873-04-01/1943-03-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (72, N'Rameau, Jean-Philippe', N'1683-09-25/1764-09-12', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (73, N'Rautavaara, Einojuhani', N'1928-10-09/open', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (74, N'Ravel, Maurice', N'1875-03-07/1937-12-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (75, N'Respighi, Ottorino', N'1879-07-09/1936-04-18', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (76, N'Rimsky-Korsakov, Nikolai', N'1844-03-18/1908-06-21', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (77, N'Rodrigo, Joaquín', N'1901-11-22/1999-07-06', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (78, N'Rossini, Gioachino', N'1792-02-29/1868-11-13', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (79, N'Saint-Saëns, Camille', N'1835-10-09/1921-12-16', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (80, N'Sarasate, Pablo de', N'1844-03-10/1908-09-20', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (81, N'Satie, Erik', N'1866-05-17/1925-07-01', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (82, N'Scarlatti, Domenico', N'1685-10-26/1757-07-23', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (83, N'Schubert, Franz', N'1797-01-31/1828-11-19', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (84, N'Schumann, Robert', N'1810-06-08/1856-07-29', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (85, N'Scriabin, Alexander', N'1872-01-06/1915-04-27', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (86, N'Shostakovich, Dmitri', N'1906-09-25/1975-08-09', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (87, N'Sibelius, Jean', N'1865-12-08/1957-09-20', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (88, N'Smetana, Bedřich', N'1824-03-02/1884-05-12', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (89, N'Strauss II, Johann', N'1825-10-25/1899-06-03', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (90, N'Strauss, Richard', N'1864-06-11/1949-09-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (91, N'Stravinsky, Igor', N'1882-06-17/1971-04-06', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (92, N'Tchaikovsky, Pyotr Illyich', N'[1840-04-25,1840-05-07]/[1893-10-25,1893-11-06]', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (93, N'Telemann, Georg Philipp', N'1681-03-14/1767-06-25', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (94, N'Vaughan Williams, Ralph', N'1872-10-12/1958-08-26', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (95, N'Verdi, Giuseppe', N'[1813-10-09,1813-10-10]/1901-01-27', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (96, N'Villa-Lobos, Heitor', N'1887-03-05/1959-11-17', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (97, N'Vivaldi, Antonio', N'1678-03-04/1741-07-28', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (98, N'Wagner, Richard', N'1813-05-22/1883-02-13', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (99, N'Walton, William', N'1902-03-29/1983-03-08', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Composer] ([ID], [Name], [Dates], [BirthLocationID], [DeathLocationID], [Biography], [IsPopular]) VALUES (100, N'Weber, Carl Maria von', N'[1786-11-18,1786-11-19]/1826-06-05', NULL, NULL, NULL, 1)
GO

INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 1)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 2)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 3)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 4)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 5)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 6)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 7)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 8)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 8)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 9)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 10)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 11)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 12)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 13)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 14)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 15)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 16)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 17)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 18)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 19)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (2, 20)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 21)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 22)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 23)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 24)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 25)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 26)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 27)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 28)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 29)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 30)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 31)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 32)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 33)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 34)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 35)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 36)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 37)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 38)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 39)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 40)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 41)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 42)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 43)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 44)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 45)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 46)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 47)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 48)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 49)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 50)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 51)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 52)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 53)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 54)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 55)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 56)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 57)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 58)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (4, 59)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 60)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 61)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 62)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 63)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 64)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 65)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 66)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 67)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 68)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 69)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 70)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 71)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 72)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 73)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 74)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 75)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 76)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 77)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 78)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 79)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 80)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 81)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 82)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 83)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 84)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 85)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 86)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 87)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 88)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 89)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 90)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 91)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 92)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 93)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 94)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 95)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 96)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (3, 97)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 98)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (6, 99)
GO
INSERT [dbo].[ComposerEra] ([EraID], [ComposerID]) VALUES (5, 100)
GO

INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (1, N'Medieval', N'0476/1400')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (2, N'Renaissance', N'1400/1600')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (3, N'Baroque', N'1600/1760')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (4, N'Classical', N'1730/1820')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (5, N'Romantic', N'1815/1910')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (6, N'20th Century', N'1900/2000')
GO
INSERT [dbo].[Era] ([ID], [Name], [Dates]) VALUES (7, N'21st Century', N'2000/open')
GO

INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (1, N'Afghan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (2, N'Albanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (3, N'Algerian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (5, N'American')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (4, N'American Samoan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (6, N'Andorran')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (7, N'Angolan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (8, N'Anguillian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (9, N'Antiguan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (10, N'Argentine')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (11, N'Armenian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (12, N'Aruban')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (13, N'Australian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (14, N'Austrian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (15, N'Azerbaijani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (16, N'Bahamian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (17, N'Bahraini')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (18, N'Bangladeshi')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (19, N'Barbadian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (20, N'Barbudan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (21, N'Basotho')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (22, N'Belarusian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (23, N'Belgian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (24, N'Belizean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (25, N'Beninese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (26, N'Bermudian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (27, N'Bhutanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (28, N'Bissau-Guinean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (29, N'Bolivian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (30, N'Bosnian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (31, N'Brazilian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (33, N'British')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (32, N'British Virgin Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (34, N'Bruneian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (35, N'Bulgarian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (36, N'Burkinabe')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (37, N'Burmese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (38, N'Burundian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (39, N'Cabo Verdean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (40, N'Cambodian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (41, N'Cameroonian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (42, N'Canadian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (43, N'Caymanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (44, N'Central African')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (45, N'Chadian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (46, N'Channel Islander (Guernsey)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (47, N'Channel Islander (Jersey)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (48, N'Chilean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (51, N'Chinese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (49, N'Chinese (Hong Kong)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (50, N'Chinese (Macau)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (52, N'Christmas Island')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (53, N'Cocos Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (54, N'Colombian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (55, N'Comoran')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (56, N'Congolese (Democratic Republic of the Congo)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (57, N'Congolese (Republic of the Congo)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (58, N'Cook Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (59, N'Costa Rican')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (60, N'Croatian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (61, N'Cuban')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (62, N'Curacaoan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (63, N'Cypriot')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (64, N'Czech')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (65, N'Danish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (66, N'Djiboutian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (67, N'Dominican (Dominica)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (68, N'Dominican (Dominican Republic)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (69, N'Dutch')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (70, N'Ecuadorian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (71, N'Egyptian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (72, N'Emirati')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (73, N'English')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (74, N'Equatorial Guinean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (75, N'Eritrean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (76, N'Estonian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (77, N'Ethiopian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (78, N'Falkland Island')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (79, N'Faroese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (80, N'Fijian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (81, N'Finnish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (83, N'French')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (82, N'French Polynesian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (84, N'Futunan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (85, N'Gabonese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (86, N'Gambian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (87, N'Georgian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (88, N'German')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (89, N'Ghanaian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (90, N'Gibraltarian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (91, N'Greek')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (92, N'Greenlandic')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (93, N'Grenadian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (94, N'Guamanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (95, N'Guatemalan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (96, N'Guinean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (97, N'Guyanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (98, N'Haitian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (99, N'Herzegovinian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (100, N'Honduran')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (101, N'Hungarian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (102, N'Icelandic')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (103, N'I-Kiribati')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (104, N'Indian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (105, N'Indonesian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (106, N'Iranian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (107, N'Iraqi')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (108, N'Irish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (109, N'Israeli')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (110, N'Italian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (111, N'Ivorian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (112, N'Jamaican')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (113, N'Japanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (114, N'Jordanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (115, N'Kazakhstani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (116, N'Kenyan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (117, N'Kittitian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (118, N'Kosovan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (119, N'Kuwaiti')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (120, N'Kyrgyzstani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (121, N'Lao')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (122, N'Latvian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (123, N'Lebanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (124, N'Liberian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (125, N'Libyan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (126, N'Liechtenstein')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (127, N'Lithuanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (128, N'Luxembourg')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (129, N'Macedonian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (130, N'Malagasy')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (131, N'Malawian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (132, N'Malaysian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (133, N'Maldivian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (134, N'Malian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (135, N'Maltese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (136, N'Manx')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (137, N'Marshallese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (138, N'Mauritanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (139, N'Mauritian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (140, N'Mexican')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (141, N'Micronesian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (142, N'Moldovan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (143, N'Monegasque')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (144, N'Mongolian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (145, N'Montenegrin')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (146, N'Montserratian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (147, N'Moroccan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (148, N'Motswana')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (149, N'Mozambican')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (150, N'Namibian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (151, N'Nauruan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (152, N'Nepali')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (153, N'Nevisian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (154, N'New Caledonian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (155, N'New Zealand')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (156, N'Nicaraguan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (157, N'Nigerian (Niger)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (158, N'Nigerian (Nigeria)')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (159, N'Niuean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (160, N'Ni-Vanuatu')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (161, N'Norfolk Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (162, N'North Korean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (163, N'Norwegian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (164, N'Omani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (165, N'Pakistani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (166, N'Palauan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (167, N'Palestinian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (168, N'Panamanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (169, N'Papua New Guinean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (170, N'Paraguayan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (171, N'Peruvian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (172, N'Philippine')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (173, N'Pitcairn Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (174, N'Polish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (175, N'Portuguese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (176, N'Puerto Rican')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (177, N'Qatari')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (178, N'Romanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (179, N'Russian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (180, N'Rwandan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (181, N'Sahrawi')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (182, N'Saint Helenian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (183, N'Saint Lucian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (184, N'Saint Vincentian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (185, N'Salvadoran')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (186, N'Sammarinese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (187, N'Samoan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (188, N'Sao Tomean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (189, N'Saudi')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (190, N'Scottish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (191, N'Senegalese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (192, N'Serbian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (193, N'Seychellois')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (194, N'Sierra Leonean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (195, N'Singapore')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (196, N'Slovak')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (197, N'Slovenian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (198, N'Solomon Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (199, N'Somali')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (200, N'South African')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (201, N'South Korean')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (202, N'South Sudanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (203, N'Spanish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (204, N'Sri Lankan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (205, N'Sudanese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (206, N'Surinamese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (207, N'Swazi')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (208, N'Swedish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (209, N'Swiss')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (210, N'Syrian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (211, N'Taiwan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (212, N'Tajikistani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (213, N'Tanzanian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (214, N'Thai')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (215, N'Timorese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (216, N'Tobagonian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (217, N'Togolese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (218, N'Tokelauan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (219, N'Tongan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (220, N'Trinidadian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (221, N'Tunisian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (222, N'Turkish')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (223, N'Turkmen')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (224, N'Tuvaluan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (225, N'Ugandan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (226, N'Ukrainian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (227, N'Unknown')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (228, N'Uruguayan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (229, N'Uzbekistani')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (230, N'Venezuelan')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (231, N'Vietnamese')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (232, N'Virgin Islander')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (233, N'Wallisian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (234, N'Welsh')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (235, N'Yemeni')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (236, N'Zambian')
GO
INSERT [dbo].[Nationality] ([ID], [Name]) VALUES (237, N'Zimbabwean')
GO

ALTER TABLE [dbo].[Era] ADD CONSTRAINT [UQ_Era_Name] UNIQUE ([Name])
GO

ALTER TABLE [dbo].[Location] ADD CONSTRAINT [UQ_Location_Name] UNIQUE ([Name])
GO

ALTER TABLE [dbo].[Nationality] ADD CONSTRAINT [UQ_Nationality_Name] UNIQUE ([Name])
GO

ALTER TABLE [dbo].[CatalogNumber] WITH NOCHECK ADD CONSTRAINT [FK_CatalogNumber_CompositionCatalogID] FOREIGN KEY ([CompositionCatalogID]) REFERENCES [dbo].[CompositionCatalog] ([ID])
GO

ALTER TABLE [dbo].[CatalogNumber] ADD CONSTRAINT [FK_CatalogNumber_CompositionCollectionID] FOREIGN KEY ([CompositionCollectionID]) REFERENCES [dbo].[CompositionCollection] ([ID])
GO

ALTER TABLE [dbo].[CatalogNumber] WITH NOCHECK ADD CONSTRAINT [FK_CatalogNumber_CompositionID] FOREIGN KEY ([CompositionID]) REFERENCES [dbo].[Composition] ([ID])
GO

ALTER TABLE [dbo].[Composer] ADD CONSTRAINT [FK_Composer_BirthLocationID] FOREIGN KEY ([BirthLocationID]) REFERENCES [dbo].[Location] ([ID])
GO

ALTER TABLE [dbo].[Composer] ADD CONSTRAINT [FK_Composer_DeathLocationID] FOREIGN KEY ([DeathLocationID]) REFERENCES [dbo].[Location] ([ID])
GO

ALTER TABLE [dbo].[ComposerEra] ADD CONSTRAINT [FK_ComposerEra_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerEra] ADD CONSTRAINT [FK_ComposerEra_EraID] FOREIGN KEY ([EraID]) REFERENCES [dbo].[Era] ([ID])
GO

ALTER TABLE [dbo].[ComposerImage] ADD CONSTRAINT [FK_ComposerImage_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerInfluence] ADD CONSTRAINT [FK_ComposerInfluence_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerInfluence] ADD CONSTRAINT [FK_ComposerInfluence_InfluenceID] FOREIGN KEY ([InfluenceID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerLink] ADD CONSTRAINT [FK_ComposerLink_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerNationality] ADD CONSTRAINT [FK_ComposerNationality_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[ComposerNationality] ADD CONSTRAINT [FK_ComposerNationality_NationalityID] FOREIGN KEY ([NationalityID]) REFERENCES [dbo].[Nationality] ([ID])
GO

ALTER TABLE [dbo].[Composition] ADD CONSTRAINT [FK_Composition_CompositionCollectionID] FOREIGN KEY([CompositionCollectionID]) REFERENCES [dbo].[CompositionCollection] ([ID])
GO

ALTER TABLE [dbo].[CompositionCatalog] ADD CONSTRAINT [FK_CompositionCatalog_ComposerID] FOREIGN KEY  ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[CompositionCollectionComposer] ADD CONSTRAINT [FK_CompositionCollectionComposer_ComposerID] FOREIGN KEY ([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[CompositionCollectionComposer] WITH NOCHECK ADD CONSTRAINT [FK_CompositionCollectionComposer_CompositionCollectionID] FOREIGN KEY ([CompositionCollectionID]) REFERENCES [dbo].[CompositionCollection] ([ID])
GO

ALTER TABLE [dbo].[CompositionComposer] ADD CONSTRAINT [FK_CompositionComposer_ComposerID] FOREIGN KEY([ComposerID]) REFERENCES [dbo].[Composer] ([ID])
GO

ALTER TABLE [dbo].[CompositionComposer] ADD CONSTRAINT [FK_CompositionComposer_CompositionID] FOREIGN KEY([CompositionID]) REFERENCES [dbo].[Composition] ([ID])
GO

ALTER TABLE [dbo].[Movement] WITH NOCHECK ADD CONSTRAINT [FK_Movement_CompositionID] FOREIGN KEY([CompositionID]) REFERENCES [dbo].[Composition] ([ID])
GO

ALTER TABLE [dbo].[Recording] WITH NOCHECK ADD CONSTRAINT [FK_Recording_AlbumID] FOREIGN KEY ([AlbumID]) REFERENCES [dbo].[Album] ([ID])
GO

ALTER TABLE [dbo].[Recording] ADD CONSTRAINT [FK_Recording_CompositionCollectionID] FOREIGN KEY ([CompositionCollectionID]) REFERENCES [dbo].[CompositionCollection] ([ID])
GO

ALTER TABLE [dbo].[Recording] ADD CONSTRAINT [FK_Recording_CompositionID] FOREIGN KEY ([CompositionID]) REFERENCES [dbo].[Composition] ([ID])
GO

ALTER TABLE [dbo].[Recording] ADD CONSTRAINT [FK_Recording_MovementID] FOREIGN KEY ([MovementID]) REFERENCES [dbo].[Movement] ([ID])
GO

ALTER TABLE [dbo].[RecordingLocation] ADD CONSTRAINT [FK_RecordingLocation_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [dbo].[Location] ([ID])
GO

ALTER TABLE [dbo].[RecordingLocation] ADD CONSTRAINT [FK_RecordingLocation_RecordingID] FOREIGN KEY ([RecordingID]) REFERENCES [dbo].[Recording] ([ID])
GO

ALTER TABLE [dbo].[RecordingPerformer] ADD CONSTRAINT [FK_RecordingPerformer_PerformerID] FOREIGN KEY ([PerformerID]) REFERENCES [dbo].[Performer] ([ID])
GO

ALTER TABLE [dbo].[RecordingPerformer] ADD CONSTRAINT [FK_RecordingPerformer_RecordingID] FOREIGN KEY ([RecordingID]) REFERENCES [dbo].[Recording] ([ID])
GO