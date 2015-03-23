DROP TABLE IF EXISTS "Era";
DROP TABLE IF EXISTS "Location";
DROP TABLE IF EXISTS "Nationality";
DROP TABLE IF EXISTS "Composer";
DROP TABLE IF EXISTS "ComposerImage";
DROP TABLE IF EXISTS "ComposerInfluence";
DROP TABLE IF EXISTS "CompositionCatalog";
DROP TABLE IF EXISTS "ComposerLink";
DROP TABLE IF EXISTS "CompositionCollection";
DROP TABLE IF EXISTS "CompositionCollectionCatalogNumber";
DROP TABLE IF EXISTS "Album";
DROP TABLE IF EXISTS "Performer";
DROP TABLE IF EXISTS "CompositionCollectionAudioFile";
DROP TABLE IF EXISTS "CompositionCollectionAudioFilePerformer";
DROP TABLE IF EXISTS "Key";
DROP TABLE IF EXISTS "Composition";
DROP TABLE IF EXISTS "CompositionCatalogNumber";
DROP TABLE IF EXISTS "CompositionAudioFile";
DROP TABLE IF EXISTS "CompositionAudioFilePerformer";
DROP TABLE IF EXISTS "Movement";
DROP TABLE IF EXISTS "MovementAudioFile";
DROP TABLE IF EXISTS "MovementAudioFilePerformer";

-- Pre-Composer (Composer References)

CREATE TABLE "Era" (
	"ID"    INTEGER NOT NULL PRIMARY KEY,
	"Name"  TEXT    NOT NULL UNIQUE
) WITHOUT ROWID;

CREATE TABLE "Location" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT  NOT NULL UNIQUE
) WITHOUT ROWID;

CREATE TABLE "Nationality" (
	"ID"   INTEGER  NOT NULL PRIMARY KEY,
	"Name" TEXT     NOT NULL UNIQUE
) WITHOUT ROWID;

-- Composer

CREATE TABLE "Composer" (
	"ID"            INTEGER NOT NULL PRIMARY KEY,
	"Name"          TEXT    NOT NULL,
	"BirthYear"     INTEGER NOT NULL,
	"DeathYear"     INTEGER,
	"BirthPlaceID"  INTEGER REFERENCES "Location"("ID"),
	"DeathPlaceID"  INTEGER REFERENCES "Location"("ID"),
	"NationalityID" INTEGER REFERENCES "Nationality"("ID"),
	"EraID"         INTEGER NOT NULL REFERENCES "Era"("ID"),
	"Biography"     TEXT,
	"IsPopular"     INTEGER NOT NULL
) WITHOUT ROWID;

-- Post-Composer (References Composer)

CREATE TABLE "ComposerImage" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"Path"       TEXT    NOT NULL UNIQUE,
	"ComposerID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "ComposerInfluence" (
	"ID"           INTEGER NOT NULL PRIMARY KEY,
	"InfluenceID"  INTEGER NOT NULL REFERENCES "Composer"("ID"),
	"InfluencedID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCatalog" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"Name"       TEXT    NOT NULL UNIQUE,
	"Prefix"     TEXT    NOT NULL UNIQUE,
	"ComposerID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "ComposerLink" (
	"ID"          INTEGER NOT NULL PRIMARY KEY,
	"URL"         TEXT    NOT NULL UNIQUE,
	"FaviconPath" TEXT,
    "ComposerID"  INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

-- Pre-CompositionCollection (CompositionCollection References)
-- CompositionCollection

CREATE TABLE "CompositionCollection" (
	"ID"                INTEGER NOT NULL PRIMARY KEY,
	"CommonName"        TEXT    NOT NULL,
	"Nickname"          TEXT,
	"TotalCompositions" INTEGER NOT NULL,
	"IsPopular"         INTEGER NOT NULL,
	"ComposerID"        INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

-- Post-CompositionCollection (References CompositionCollection)

CREATE TABLE "CompositionCollectionCatalogNumber" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Value"                   INTEGER NOT NULL,
	"CompositionCatalogID"    INTEGER NOT NULL REFERENCES "CompositionCatalog"("ID"),
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

CREATE TABLE "Album" (
	"ID"          INTEGER NOT NULL PRIMARY KEY,
	"Name"        TEXT    NOT NULL,
	"TotalTracks" INTEGER NOT NULL
) WITHOUT ROWID;

CREATE TABLE "Performer" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL
) WITHOUT ROWID;

CREATE TABLE "CompositionCollectionAudioFile" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Path"                    TEXT    NOT NULL UNIQUE,
        "RecordingCompletionDate" TEXT,
	"RecordingLocationID"     INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	          INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"             INTEGER,
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCollectionAudioFilePerformer" (
        "ID"				   INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"                      INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"CompositionCollectionAudioFileID" INTEGER NOT NULL REFERENCES "CompositionCollectionAudioFile"("ID")
) WITHOUT ROWID;

-- Pre-Composition (Composition References)

CREATE TABLE "Key" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL UNIQUE
) WITHOUT ROWID;

-- Composition

CREATE TABLE "Composition" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"CommonName"              TEXT    NOT NULL,
	"KeyID"                   INTEGER REFERENCES "Key"("ID"),
	"CompletionYear"          INTEGER NOT NULL,
	"Nickname"                TEXT,
	"TotalMovements"          INTEGER NOT NULL,
	"IsPopular"               INTEGER NOT NULL,
	"ComposerID"              INTEGER NOT NULL REFERENCES "Composer"("ID"),
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

-- Post-Composition (References Composition)

CREATE TABLE "CompositionCatalogNumber" (
	"ID"                   INTEGER NOT NULL PRIMARY KEY,
	"Value"                INTEGER NOT NULL,
	"CompositionCatalogID" INTEGER NOT NULL REFERENCES "CompositionCatalog"("ID"),
	"CompositionID"        INTEGER NOT NULL REFERENCES "Composition"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionAudioFile" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Path"                    TEXT    NOT NULL UNIQUE,
	"RecordingCompletionDate" TEXT,
	"RecordingLocationID"     INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	          INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"             INTEGER,
	"CompositionID"           INTEGER NOT NULL REFERENCES "Composition"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionAudioFilePerformer" (
	"ID"                     INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"            INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"CompositionAudioFileID" INTEGER NOT NULL REFERENCES "CompositionAudioFile"("ID")
) WITHOUT ROWID;

-- Pre-Movement (Movement References)
-- Movement

CREATE TABLE "Movement" (
	"ID"            INTEGER NOT NULL PRIMARY KEY,
	"CommonName"    TEXT    NOT NULL,
	"Number"        INTEGER NOT NULL,
	"KeyID"         INTEGER REFERENCES "Key"("ID"),
	"CompositionID" INTEGER NOT NULL REFERENCES "Composition"("ID"),
	"IsPopular"     INTEGER NOT NULL
) WITHOUT ROWID;

-- Post-Movement (References Movement)

CREATE TABLE "MovementAudioFile" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Path"                    TEXT    NOT NULL UNIQUE,
        "RecordingCompletionDate" TEXT,
	"RecordingLocationID"     INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	              INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"             INTEGER,
	"MovementID"              INTEGER NOT NULL REFERENCES "Movement"("ID")
) WITHOUT ROWID;

CREATE TABLE "MovementAudioFilePerformer" (
	"ID"                  INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"         INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"MovementAudioFileID" INTEGER NOT NULL REFERENCES "MovementAudioFile"("ID")
) WITHOUT ROWID;