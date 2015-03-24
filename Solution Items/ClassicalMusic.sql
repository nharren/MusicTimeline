CREATE TABLE "Era" (
	"ID"    INTEGER NOT NULL PRIMARY KEY,
	"Name"  TEXT    NOT NULL UNIQUE,
	"Dates" TEXT    NOT NULL
) WITHOUT ROWID;

CREATE TABLE "Location" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL UNIQUE
) WITHOUT ROWID;

CREATE TABLE "Nationality" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL UNIQUE
) WITHOUT ROWID;

CREATE TABLE "Composer" (
	"ID"              INTEGER NOT NULL PRIMARY KEY,
	"Name"            TEXT    NOT NULL,
	"Dates"           TEXT    NOT NULL,
	"BirthLocationID" INTEGER REFERENCES "Location"("ID"),
	"DeathLocationID" INTEGER REFERENCES "Location"("ID"),
	"NationalityID"   INTEGER REFERENCES "Nationality"("ID"),
	"Biography"       TEXT,
	"IsPopular"       INTEGER NOT NULL
) WITHOUT ROWID;

CREATE TABLE "ComposerEra" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"EraID"      INTEGER NOT NULL REFERENCES "Era" ("ID"),
	"ComposerID" INTEGER NOT NULL REFERENCES "Composer" ("ID")
) WITHOUT ROWID;

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

CREATE TABLE "CatalogNumberPrefix" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"Prefix"     TEXT    NOT NULL UNIQUE,
	"ComposerID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "ComposerLink" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"URL"        TEXT    NOT NULL UNIQUE,
    "ComposerID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCollection" (
	"ID"         INTEGER NOT NULL PRIMARY KEY,
	"Name"       TEXT    NOT NULL,
	"IsPopular"  INTEGER NOT NULL,
	"ComposerID" INTEGER NOT NULL REFERENCES "Composer"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCollectionCatalogNumber" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Value"                   INTEGER NOT NULL,
	"CompositionCatalogID"    INTEGER NOT NULL REFERENCES "CompositionCatalog"("ID"),
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

CREATE TABLE "Album" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL
) WITHOUT ROWID;

CREATE TABLE "Performer" (
	"ID"   INTEGER NOT NULL PRIMARY KEY,
	"Name" TEXT    NOT NULL
) WITHOUT ROWID;

CREATE TABLE "CompositionCollectionAudioFile" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Path"                    TEXT    NOT NULL UNIQUE,
    "RecordingDates"          TEXT    NOT NULL,
	"RecordingLocationID"     INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	              INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"             INTEGER,
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCollectionAudioFilePerformer" (
    "ID"				               INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"                      INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"CompositionCollectionAudioFileID" INTEGER NOT NULL REFERENCES "CompositionCollectionAudioFile"("ID")
) WITHOUT ROWID;

CREATE TABLE "Composition" (
	"ID"                      INTEGER NOT NULL PRIMARY KEY,
	"Name"                    TEXT    NOT NULL,
	"Dates"                   TEXT    NOT NULL,
	"Nickname"                TEXT,
	"IsPopular"               INTEGER NOT NULL,
	"ComposerID"              INTEGER NOT NULL REFERENCES "Composer"("ID"),
	"CompositionCollectionID" INTEGER NOT NULL REFERENCES "CompositionCollection"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionCatalogNumber" (
	"ID"                   INTEGER NOT NULL PRIMARY KEY,
	"Value"                INTEGER NOT NULL,
	"CompositionCatalogID" INTEGER NOT NULL REFERENCES "CompositionCatalog"("ID"),
	"CompositionID"        INTEGER NOT NULL REFERENCES "Composition"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionAudioFile" (
	"ID"                  INTEGER NOT NULL PRIMARY KEY,
	"Path"                TEXT    NOT NULL UNIQUE,
	"RecordingDates"      TEXT    NOT NULL,
	"RecordingLocationID" INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	          INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"         INTEGER,
	"CompositionID"       INTEGER NOT NULL REFERENCES "Composition"("ID")
) WITHOUT ROWID;

CREATE TABLE "CompositionAudioFilePerformer" (
	"ID"                     INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"            INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"CompositionAudioFileID" INTEGER NOT NULL REFERENCES "CompositionAudioFile"("ID")
) WITHOUT ROWID;

CREATE TABLE "Movement" (
	"ID"            INTEGER NOT NULL PRIMARY KEY,
	"Name"          TEXT    NOT NULL,
	"Number"        INTEGER NOT NULL,
	"CompositionID" INTEGER NOT NULL REFERENCES "Composition"("ID"),
	"IsPopular"     INTEGER NOT NULL
) WITHOUT ROWID;

CREATE TABLE "MovementAudioFile" (
	"ID"                  INTEGER NOT NULL PRIMARY KEY,
	"Path"                TEXT    NOT NULL UNIQUE,
    "RecordingDates"      TEXT    NOT NULL,
	"RecordingLocationID" INTEGER REFERENCES "Location"("ID"),
	"AlbumID"	          INTEGER REFERENCES "Album"("ID"),
	"TrackNumber"         INTEGER,
	"MovementID"          INTEGER NOT NULL REFERENCES "Movement"("ID")
) WITHOUT ROWID;

CREATE TABLE "MovementAudioFilePerformer" (
	"ID"                  INTEGER NOT NULL PRIMARY KEY,
	"PerformerID"         INTEGER NOT NULL REFERENCES "Performer"("ID"),
	"MovementAudioFileID" INTEGER NOT NULL REFERENCES "MovementAudioFile"("ID")
) WITHOUT ROWID;

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('0','Medieval','476/1400');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('1','Renaissance','1400/1600');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('2','Baroque','1600/1760');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('3','Classical','1730/1820');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('4','Romantic','1815/1910');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('5','20th Century','1900/2000');

INSERT INTO "Era" ("ID","Name","Dates")
VALUES ('6','21st Century','2000/open',);

INSERT INTO "Nationality" ("ID","Name")
VALUES ('0','Afghan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('1','Albanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('2','Algerian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('3','American Samoan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('4','American');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('5','Andorran');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('6','Angolan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('7','Anguillian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('8','Antiguan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('9','Argentine');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('10','Armenian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('11','Aruban');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('12','Australian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('13','Austrian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('14','Azerbaijani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('15','Bahamian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('16','Bahraini');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('17','Bangladeshi');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('18','Barbadian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('19','Barbudan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('20','Basotho');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('21','Belarusian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('22','Belgian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('23','Belizean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('24','Beninese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('25','Bermudian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('26','Bhutanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('27','Bissau-Guinean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('28','Bolivian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('29','Bosnian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('30','Brazilian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('31','British Virgin Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('32','British');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('33','Bruneian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('34','Bulgarian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('35','Burkinabe');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('36','Burmese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('37','Burundian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('38','Cabo Verdean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('39','Cambodian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('40','Cameroonian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('41','Canadian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('42','Caymanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('43','Central African');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('44','Chadian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('45','Channel Islander (Guernsey)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('46','Channel Islander (Jersey)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('47','Chilean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('48','Chinese (Hong Kong)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('49','Chinese (Macau)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('50','Chinese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('51','Christmas Island');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('52','Cocos Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('53','Colombian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('54','Comoran');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('55','Congolese (Democratic Republic of the Congo)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('56','Congolese (Republic of the Congo)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('57','Cook Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('58','Costa Rican');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('59','Croatian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('60','Cuban');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('61','Curacaoan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('62','Cypriot');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('63','Czech');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('64','Danish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('65','Djiboutian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('66','Dominican (Dominica)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('67','Dominican (Dominican Republic)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('68','Dutch');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('69','Ecuadorian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('70','Egyptian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('71','Emirati');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('72','English');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('73','Equatorial Guinean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('74','Eritrean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('75','Estonian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('76','Ethiopian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('77','Falkland Island');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('78','Faroese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('79','Fijian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('80','Finnish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('81','French Polynesian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('82','French');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('83','Futunan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('84','Gabonese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('85','Gambian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('86','Georgian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('87','German');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('88','Ghanaian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('89','Gibraltarian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('90','Greek');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('91','Greenlandic');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('92','Grenadian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('93','Guamanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('94','Guatemalan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('95','Guinean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('96','Guyanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('97','Haitian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('98','Herzegovinian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('99','Honduran');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('100','Hungarian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('101','Icelandic');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('102','I-Kiribati');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('103','Indian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('104','Indonesian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('105','Iranian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('106','Iraqi');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('107','Irish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('108','Israeli');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('109','Italian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('110','Ivorian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('111','Jamaican');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('112','Japanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('113','Jordanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('114','Kazakhstani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('115','Kenyan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('116','Kittitian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('117','Kosovan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('118','Kuwaiti');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('119','Kyrgyzstani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('120','Lao');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('121','Latvian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('122','Lebanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('123','Liberian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('124','Libyan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('125','Liechtenstein');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('126','Lithuanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('127','Luxembourg');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('128','Macedonian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('129','Malagasy');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('130','Malawian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('131','Malaysian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('132','Maldivian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('133','Malian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('134','Maltese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('135','Manx');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('136','Marshallese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('137','Mauritanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('138','Mauritian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('139','Mexican');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('140','Micronesian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('141','Moldovan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('142','Monegasque');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('143','Mongolian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('144','Montenegrin');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('145','Montserratian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('146','Moroccan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('147','Motswana');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('148','Mozambican');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('149','Namibian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('150','Nauruan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('151','Nepali');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('152','Nevisian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('153','New Caledonian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('154','New Zealand');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('155','Nicaraguan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('156','Nigerian (Niger)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('157','Nigerian (Nigeria)');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('158','Niuean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('159','Ni-Vanuatu');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('160','Norfolk Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('161','North Korean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('162','Norwegian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('163','Omani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('164','Pakistani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('165','Palauan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('166','Palestinian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('167','Panamanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('168','Papua New Guinean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('169','Paraguayan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('170','Peruvian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('171','Philippine');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('172','Pitcairn Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('173','Polish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('174','Portuguese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('175','Puerto Rican');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('176','Qatari');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('177','Romanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('178','Russian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('179','Rwandan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('180','Sahrawi');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('181','Saint Helenian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('182','Saint Lucian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('183','Saint Vincentian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('184','Salvadoran');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('185','Sammarinese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('186','Samoan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('187','Sao Tomean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('188','Saudi');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('189','Scottish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('190','Senegalese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('191','Serbian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('192','Seychellois');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('193','Sierra Leonean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('194','Singapore');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('195','Slovak');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('196','Slovenian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('197','Solomon Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('198','Somali');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('199','South African');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('200','South Korean');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('201','South Sudanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('202','Spanish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('203','Sri Lankan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('204','Sudanese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('205','Surinamese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('206','Swazi');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('207','Swedish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('208','Swiss');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('209','Syrian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('210','Taiwan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('211','Tajikistani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('212','Tanzanian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('213','Thai');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('214','Timorese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('215','Tobagonian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('216','Togolese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('217','Tokelauan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('218','Tongan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('219','Trinidadian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('220','Tunisian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('221','Turkish');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('222','Turkmen');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('223','Tuvaluan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('224','Ugandan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('225','Ukrainian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('226','Unknown');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('227','Uruguayan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('228','Uzbekistani');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('229','Venezuelan');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('230','Vietnamese');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('231','Virgin Islander');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('232','Wallisian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('233','Welsh');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('234','Yemeni');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('235','Zambian');

INSERT INTO "Nationality" ("ID","Name")
VALUES ('236','Zimbabwean');


INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('0','Albéniz, Isaac','1860-05-29/1909-05-18','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('0','4','0');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('1','Albinoni, Tomaso','1671-06-08/1751-01-17','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('1','2','1');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('2','Arnold, Malcolm','1921-10-21/2006-09-23','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('2','5','2');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('3','Bach, Johann Christian','1735-09-05/1782-01-01','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('3','3','3');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('4','Bach, Johann Sebastian','1685-03-31/1750-07-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('4','2','4');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('5','Barber, Samuel','1910-03-09/1981-01-23','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('5','5','5');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('6','Bartók, Béla','1881-03-25/1945-09-26','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('6','5','6');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('7','Beethoven, Ludwig van','1770-12-17/1827-03-26','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('7','3','7');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('8','4','7');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('8','Bellini, Vincenzo','1801-11-03/1835-09-23','','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('9','4','8');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('9','Berlioz, Hector','1803-12-11/1869-03-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('10','4','9');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('10','Bernstein, Leonard','1918-08-25/1990-10-14','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('11','5','10');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('11','Bizet, Georges','1838-10-25/1875-06-03','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('12','4','11');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('12','Bloch, Ernest','1885-07-24/1959-07-15','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('13','5','12');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('13','Boccherini, Luigi','1743-02-19/1805-05-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('14','3','13');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('14','Borodin, Alexander','1833-11-12/1887-02-27','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('15','4','14');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('15','Brahms, Johannes','1833-05-07/1897-04-03','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('16','4','15');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('16','Britten, Benjamin','1913-11-22/1976-12-04','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('17','5','16');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('17','Bruch, Max','1838-01-06/1920-10-02','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('18','5','17');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('18','Bruckner, Anton','1824-09-04/1896-10-11','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('19','4','18');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('19','Byrd, William','[1539,1540,1543]/1623-07-14','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('20','1','19');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('20','Chopin, Frédéric','[1810-02-22,1810-03-01]/1849-10-17','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('21','4','20');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('21','Copland, Aaron','1900-11-14/1990-12-02','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('22','5','21');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('22','Corelli, Arcangelo','1653-02-17/1713-01-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('23','2','22');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('23','Debussy, Claude','1862-08-22/1918-03-25','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('24','4','23');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('24','Delibes, Léo','1836-02-21/1891-01-16','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('25','4','24');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('25','Delius, Frederick','1862-01-29/1934-06-10','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('26','5','25');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('26','Donizetti, Gaetano','1797-11-29/1848-04-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('27','4','26');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('27','Dvořák, Antonín','1841-09-08/1904-05-01','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('28','4','27');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('28','Elgar, Edward','1857-06-02/1934-02-23','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('29','4','28');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('29','Falla, Manuel de','1876-11-23/1946-11-14','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('30','5','29');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('30','Fauré, Gabriel','1845-05-12/1924-11-04','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('31','4','30');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('31','Franck, César','1822-12-10/1890-11-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('32','4','31');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('32','Gershwin, George','1898-09-26/1937-07-11','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('33','5','32');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('33','Glazunov, Alexander','1865-08-10/1936-03-21','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('34','4','33');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('34','Gluck, Christoph Willibald','1714-07-02/1787-11-15','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('35','3','34');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('35','Gounod, Charles','1818-06-17/[1893-10-17,1893-10-18]','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('36','4','35');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('36','Granados, Enrique','1867-07-27/1916-03-24','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('37','4','36');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('37','Grieg, Edvard','1843-06-15/1907-09-04','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('38','4','37');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('38','Handel, George Frideric','1685-02-23/1759-04-14','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('39','2','38');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('39','Haydn, Joseph','1732-03-31/1809-05-31','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('40','3','9');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('40','Hindemith, Paul','1895-11-16/1963-12-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('41','5','40');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('41','Holst, Gustav','1874-09-21/1934-05-25','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('42','5','41');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('42','Hummel, Johann Nepomuk','1778-11-14/1837-10-17','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('43','3','42');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('43','Ives, Charles','1874-10-20/1954-05-19','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('44','5','43');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('44','Janáček, Leoš','1854-07-03/1928-08-12','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('45','5','44');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('45','Khachaturain, Aram','1903-06-06/1978-05-01','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('46','5','45');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('46','Korngold, Erich Wolfgang','1897-05-29/1957-11-29','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('47','5','46');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('47','Kreisler, Fritz','1875-02-02/1962-01-29','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('48','5','47');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('48','Lehár, Franz','1870-04-30/1948-10-24','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('49','5','48');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('49','Leoncavallo, Ruggero','1857-04-23/1919-08-09','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('50','4','49');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('50','Liszt, Franz','1811-10-22/1886-07-31','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('51','4','50');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('51','Lully, Jean-Baptiste','1632-11-28/1687-03-22','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('52','2','51');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('52','Mahler, Gustav','1860-07-07/1911-05-18','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('53','4','52');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('53','Marais, Marin','1656-05-31/1728-08-15','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('54','2','53');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('54','Mascagni, Pietro','1863-12-07/1945-08-02','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('55','4','54');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('55','Massenet, Jules','1842-05-12/1912-08-13','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('56','4','55');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('56','Mendelssohn, Felix','1809-02-03/1847-11-04','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('57','4','56');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('57','Monteverdi, Claudio','1567-05-15~/1643-11-29','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('58','2','57');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('58','Mozart, Wolfgang Amadeus','1756-01-27/1791-12-05','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('59','3','58');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('59','Mussorgsky, Modest','1839-03-21/1881-03-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('60','4','59');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('60','Nielsen, Carl','1865-06-09/1931-10-03','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('61','5','60');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('61','Offenbach, Jacques','1819-06-20/1880-10-05','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('62','4','61');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('62','Pachelbel, Johann','1653-09-01~/1706-03-09~','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('63','2','62');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('63','Paganini, Niccolò','1782-10-27/1840-05-27','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('64','4','63');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('64','Ponce, Manuel','1882-12-08/1948-04-24','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('65','5','64');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('65','Poulenc, Francis','1899-01-07/1963-01-30','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('66','5','64');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('66','Prokofiev, Sergei','1891-04-27/1953-03-05','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('67','5','66');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('67','Puccini, Giacomo','1858-12-22/1924-11-29','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('68','4','67');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('68','Purcell, Henry','(1659-09-10)?/1695-11-21','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('69','2','68');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('69','Pärt, Arvo','1935-09-11/open','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('70','5','69');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('70','Rachmaninoff, Sergei','1873-04-01/1943-03-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('71','5','70');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('71','Rameau, Jean-Philippe','1683-09-25/1764-09-12','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('72','2','71');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('72','Rautavaara, Einojuhani','1928-10-09/open','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('73','5','72');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('73','Ravel, Maurice','1875-03-07/1937-12-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('74','5','73');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('74','Respighi, Ottorino','1879-07-09/1936-04-18','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('75','5','74');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('75','Rimsky-Korsakov, Nikolai','1844-03-18/1908-06-21','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('76','4','75');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('76','Rodrigo, Joaquín','1901-11-22/1999-07-06','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('77','5','76');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('77','Rossini, Gioachino','1792-02-29/1868-11-13','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('78','4','77');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('78','Saint-Saëns, Camille','1835-10-09/1921-12-16','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('79','4','78');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('79','Sarasate, Pablo de','1844-03-10/1908-09-20','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('80','4','79');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('80','Satie, Erik','1866-05-17/1925-07-01','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('81','5','80');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('81','Scarlatti, Domenico','1685-10-26/1757-07-23','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('82','2','81');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('82','Schubert, Franz','1797-01-31','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('83','4','82');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('83','Schumann, Robert','1810-06-08/1856-07-29','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('84','4','83');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('84','Scriabin, Alexander','1872-01-06/1915-04-27','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('85','5','84');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('85','Shostakovich, Dmitri','1906-09-25','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('86','5','85');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('86','Sibelius, Jean','1865-12-08/1957-09-20','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('87','5','86');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('87','Smetana, Bedřich','1824-03-02/1884-05-12','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('88','4','87');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('88','Strauss II, Johann','1825-10-25/1899-06-03','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('89','4','88');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('89','Strauss, Richard','1864-06-11/1949-09-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('90','5','89');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('90','Stravinsky, Igor','1882-06-17/1971-04-06','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('91','5','90');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('91','Tchaikovsky, Pyotr Illyich','[1840-04-25,1840-05-07]/[1893-10-25,1893-11-6]','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('92','4','91');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('92','Telemann, Georg Philipp','1681-03-14/1767-06-25','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('93','2','92');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('93','Vaughan Williams, Ralph','1872-10-12/1958-08-26','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('94','5','93');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('94','Verdi, Giuseppe','[1813-10-09,1813-10-10]/1901-01-27','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('95','4','94');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('95','Villa-Lobos, Heitor','1887-03-05','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('96','5','95');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('96','Vivaldi, Antonio','1678-03-04/1741-07-28','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('97','2','96');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('97','Wagner, Richard','1813-05-22/1883-02-13','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('98','4','97');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('98','Walton, William','1902-03-29/1983-03-08','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('99','5','98');



INSERT INTO "Composer" ("ID","Name","Dates","IsPopular")
VALUES ('99','Weber, Carl Maria von','[1786-11-18,1786-11-19]/1826-06-05','1');

INSERT INTO "ComposerEra" ("ID", "EraID", "ComposerID")
VALUES ('100','4','99');