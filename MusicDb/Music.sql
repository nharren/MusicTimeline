-- MySQL dump 10.13  Distrib 5.6.17, for Win32 (x86)
--
-- Host: localhost    Database: music_test
-- ------------------------------------------------------
-- Server version	5.6.23-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `album`
--

DROP TABLE IF EXISTS `album`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `album` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `album`
--

LOCK TABLES `album` WRITE;
/*!40000 ALTER TABLE `album` DISABLE KEYS */;
/*!40000 ALTER TABLE `album` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `catalog_number`
--

DROP TABLE IF EXISTS `catalog_number`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `catalog_number` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `number` varchar(15) NOT NULL,
  `composition_catalog_id` smallint(5) unsigned NOT NULL,
  `composition_collection_id` smallint(5) unsigned NOT NULL,
  `composition_id` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `composition_catalog_id` (`composition_catalog_id`),
  KEY `composition_collection_id` (`composition_collection_id`),
  KEY `composition_id` (`composition_id`),
  CONSTRAINT `catalog_number_ibfk_1` FOREIGN KEY (`composition_catalog_id`) REFERENCES `composition_catalog` (`id`),
  CONSTRAINT `catalog_number_ibfk_2` FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`),
  CONSTRAINT `catalog_number_ibfk_3` FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `catalog_number`
--

LOCK TABLES `catalog_number` WRITE;
/*!40000 ALTER TABLE `catalog_number` DISABLE KEYS */;
/*!40000 ALTER TABLE `catalog_number` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer`
--

DROP TABLE IF EXISTS `composer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `dates` varchar(50) NOT NULL,
  `birth_location_id` mediumint(8) unsigned DEFAULT NULL,
  `death_location_id` mediumint(8) unsigned DEFAULT NULL,
  `biography` text,
  `is_popular` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `birth_location_id` (`birth_location_id`),
  KEY `death_location_id` (`death_location_id`),
  CONSTRAINT `composer_ibfk_1` FOREIGN KEY (`birth_location_id`) REFERENCES `location` (`id`),
  CONSTRAINT `composer_ibfk_2` FOREIGN KEY (`death_location_id`) REFERENCES `location` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer`
--

LOCK TABLES `composer` WRITE;
/*!40000 ALTER TABLE `composer` DISABLE KEYS */;
INSERT INTO `composer` VALUES (1,'Albéniz, Isaac','1860-05-29/1909-05-18',NULL,NULL,NULL,1),(2,'Albinoni, Tomaso','1671-06-08/1751-01-17',NULL,NULL,NULL,1),(3,'Arnold, Malcolm','1921-10-21/2006-09-23',NULL,NULL,NULL,1),(4,'Bach, Johann Christian','1735-09-05/1782-01-01',NULL,NULL,NULL,1),(5,'Bach, Johann Sebastian','1685-03-31/1750-07-28',NULL,NULL,NULL,1),(6,'Barber, Samuel','1910-03-09/1981-01-23',NULL,NULL,NULL,1),(7,'Bartók, Béla','1881-03-25/1945-09-26',NULL,NULL,NULL,1),(8,'Beethoven, Ludwig van','1770-12-17/1827-03-26',NULL,NULL,NULL,1),(9,'Bellini, Vincenzo','1801-11-03/1835-09-23',NULL,NULL,NULL,1),(10,'Berlioz, Hector','1803-12-11/1869-03-08',NULL,NULL,NULL,1),(11,'Bernstein, Leonard','1918-08-25/1990-10-14',NULL,NULL,NULL,1),(12,'Bizet, Georges','1838-10-25/1875-06-03',NULL,NULL,NULL,1),(13,'Bloch, Ernest','1885-07-24/1959-07-15',NULL,NULL,NULL,1),(14,'Boccherini, Luigi','1743-02-19/1805-05-28',NULL,NULL,NULL,1),(15,'Borodin, Alexander','1833-11-12/1887-02-27',NULL,NULL,NULL,1),(16,'Brahms, Johannes','1833-05-07/1897-04-03',NULL,NULL,NULL,1),(17,'Britten, Benjamin','1913-11-22/1976-12-04',NULL,NULL,NULL,1),(18,'Bruch, Max','1838-01-06/1920-10-02',NULL,NULL,NULL,1),(19,'Bruckner, Anton','1824-09-04/1896-10-11',NULL,NULL,NULL,1),(20,'Byrd, William','[1539,1540,1543]/1623-07-14',NULL,NULL,NULL,1),(21,'Chopin, Frédéric','[1810-02-22,1810-03-01]/1849-10-17',NULL,NULL,NULL,1),(22,'Copland, Aaron','1900-11-14/1990-12-02',NULL,NULL,NULL,1),(23,'Corelli, Arcangelo','1653-02-17/1713-01-08',NULL,NULL,NULL,1),(24,'Debussy, Claude','1862-08-22/1918-03-25',NULL,NULL,NULL,1),(25,'Delibes, Léo','1836-02-21/1891-01-16',NULL,NULL,NULL,1),(26,'Delius, Frederick','1862-01-29/1934-06-10',NULL,NULL,NULL,1),(27,'Donizetti, Gaetano','1797-11-29/1848-04-08',NULL,NULL,NULL,1),(28,'Dvořák, Antonín','1841-09-08/1904-05-01',NULL,NULL,NULL,1),(29,'Elgar, Edward','1857-06-02/1934-02-23',NULL,NULL,NULL,1),(30,'Falla, Manuel de','1876-11-23/1946-11-14',NULL,NULL,NULL,1),(31,'Fauré, Gabriel','1845-05-12/1924-11-04',NULL,NULL,NULL,1),(32,'Franck, César','1822-12-10/1890-11-08',NULL,NULL,NULL,1),(33,'Gershwin, George','1898-09-26/1937-07-11',NULL,NULL,NULL,1),(34,'Glazunov, Alexander','1865-08-10/1936-03-21',NULL,NULL,NULL,1),(35,'Gluck, Christoph Willibald','1714-07-02/1787-11-15',NULL,NULL,NULL,1),(36,'Gounod, Charles','1818-06-17/[1893-10-17,1893-10-18]',NULL,NULL,NULL,1),(37,'Granados, Enrique','1867-07-27/1916-03-24',NULL,NULL,NULL,1),(38,'Grieg, Edvard','1843-06-15/1907-09-04',NULL,NULL,NULL,1),(39,'Handel, George Frideric','1685-02-23/1759-04-14',NULL,NULL,NULL,1),(40,'Haydn, Joseph','1732-03-31/1809-05-31',NULL,NULL,NULL,1),(41,'Hindemith, Paul','1895-11-16/1963-12-28',NULL,NULL,NULL,1),(42,'Holst, Gustav','1874-09-21/1934-05-25',NULL,NULL,NULL,1),(43,'Hummel, Johann Nepomuk','1778-11-14/1837-10-17',NULL,NULL,NULL,1),(44,'Ives, Charles','1874-10-20/1954-05-19',NULL,NULL,NULL,1),(45,'Janáček, Leoš','1854-07-03/1928-08-12',NULL,NULL,NULL,1),(46,'Khachaturain, Aram','1903-06-06/1978-05-01',NULL,NULL,NULL,1),(47,'Korngold, Erich Wolfgang','1897-05-29/1957-11-29',NULL,NULL,NULL,1),(48,'Kreisler, Fritz','1875-02-02/1962-01-29',NULL,NULL,NULL,1),(49,'Lehár, Franz','1870-04-30/1948-10-24',NULL,NULL,NULL,1),(50,'Leoncavallo, Ruggero','1857-04-23/1919-08-09',NULL,NULL,NULL,1),(51,'Liszt, Franz','1811-10-22/1886-07-31',NULL,NULL,NULL,1),(52,'Lully, Jean-Baptiste','1632-11-28/1687-03-22',NULL,NULL,NULL,1),(53,'Mahler, Gustav','1860-07-07/1911-05-18',NULL,NULL,NULL,1),(54,'Marais, Marin','1656-05-31/1728-08-15',NULL,NULL,NULL,1),(55,'Mascagni, Pietro','1863-12-07/1945-08-02',NULL,NULL,NULL,1),(56,'Massenet, Jules','1842-05-12/1912-08-13',NULL,NULL,NULL,1),(57,'Mendelssohn, Felix','1809-02-03/1847-11-04',NULL,NULL,NULL,1),(58,'Monteverdi, Claudio','1567-05-15~/1643-11-29',NULL,NULL,NULL,1),(59,'Mozart, Wolfgang Amadeus','1756-01-27/1791-12-05',NULL,NULL,NULL,1),(60,'Mussorgsky, Modest','1839-03-21/1881-03-28',NULL,NULL,NULL,1),(61,'Nielsen, Carl','1865-06-09/1931-10-03',NULL,NULL,NULL,1),(62,'Offenbach, Jacques','1819-06-20/1880-10-05',NULL,NULL,NULL,1),(63,'Pachelbel, Johann','1653-09-01~/1706-03-09~',NULL,NULL,NULL,1),(64,'Paganini, Niccolò','1782-10-27/1840-05-27',NULL,NULL,NULL,1),(65,'Ponce, Manuel','1882-12-08/1948-04-24',NULL,NULL,NULL,1),(66,'Poulenc, Francis','1899-01-07/1963-01-30',NULL,NULL,NULL,1),(67,'Prokofiev, Sergei','1891-04-27/1953-03-05',NULL,NULL,NULL,1),(68,'Puccini, Giacomo','1858-12-22/1924-11-29',NULL,NULL,NULL,1),(69,'Purcell, Henry','(1659-09-10)?/1695-11-21',NULL,NULL,NULL,1),(70,'Pärt, Arvo','1935-09-11/open',NULL,NULL,NULL,1),(71,'Rachmaninoff, Sergei','1873-04-01/1943-03-28',NULL,NULL,NULL,1),(72,'Rameau, Jean-Philippe','1683-09-25/1764-09-12',NULL,NULL,NULL,1),(73,'Rautavaara, Einojuhani','1928-10-09/open',NULL,NULL,NULL,1),(74,'Ravel, Maurice','1875-03-07/1937-12-28',NULL,NULL,NULL,1),(75,'Respighi, Ottorino','1879-07-09/1936-04-18',NULL,NULL,NULL,1),(76,'Rimsky-Korsakov, Nikolai','1844-03-18/1908-06-21',NULL,NULL,NULL,1),(77,'Rodrigo, Joaquín','1901-11-22/1999-07-06',NULL,NULL,NULL,1),(78,'Rossini, Gioachino','1792-02-29/1868-11-13',NULL,NULL,NULL,1),(79,'Saint-Saëns, Camille','1835-10-09/1921-12-16',NULL,NULL,NULL,1),(80,'Sarasate, Pablo de','1844-03-10/1908-09-20',NULL,NULL,NULL,1),(81,'Satie, Erik','1866-05-17/1925-07-01',NULL,NULL,NULL,1),(82,'Scarlatti, Domenico','1685-10-26/1757-07-23',NULL,NULL,NULL,1),(83,'Schubert, Franz','1797-01-31/1828-11-19',NULL,NULL,NULL,1),(84,'Schumann, Robert','1810-06-08/1856-07-29',NULL,NULL,NULL,1),(85,'Scriabin, Alexander','1872-01-06/1915-04-27',NULL,NULL,NULL,1),(86,'Shostakovich, Dmitri','1906-09-25/1975-08-09',NULL,NULL,NULL,1),(87,'Sibelius, Jean','1865-12-08/1957-09-20',NULL,NULL,NULL,1),(88,'Smetana, Bedřich','1824-03-02/1884-05-12',NULL,NULL,NULL,1),(89,'Strauss II, Johann','1825-10-25/1899-06-03',NULL,NULL,NULL,1),(90,'Strauss, Richard','1864-06-11/1949-09-08',NULL,NULL,NULL,1),(91,'Stravinsky, Igor','1882-06-17/1971-04-06',NULL,NULL,NULL,1),(92,'Tchaikovsky, Pyotr Illyich','[1840-04-25,1840-05-07]/[1893-10-25,1893-11-06]',NULL,NULL,NULL,1),(93,'Telemann, Georg Philipp','1681-03-14/1767-06-25',NULL,NULL,NULL,1),(94,'Vaughan Williams, Ralph','1872-10-12/1958-08-26',NULL,NULL,NULL,1),(95,'Verdi, Giuseppe','[1813-10-09,1813-10-10]/1901-01-27',NULL,NULL,NULL,1),(96,'Villa-Lobos, Heitor','1887-03-05/1959-11-17',NULL,NULL,NULL,1),(97,'Vivaldi, Antonio','1678-03-04/1741-07-28',NULL,NULL,NULL,1),(98,'Wagner, Richard','1813-05-22/1883-02-13',NULL,NULL,NULL,1),(99,'Walton, William','1902-03-29/1983-03-08',NULL,NULL,NULL,1),(100,'Weber, Carl Maria von','[1786-11-18,1786-11-19]/1826-06-05',NULL,NULL,NULL,1);
/*!40000 ALTER TABLE `composer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer_era`
--

DROP TABLE IF EXISTS `composer_era`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer_era` (
  `era_id` tinyint(1) unsigned NOT NULL,
  `composer_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`era_id`,`composer_id`),
  KEY `composer_id` (`composer_id`),
  CONSTRAINT `composer_era_ibfk_1` FOREIGN KEY (`era_id`) REFERENCES `era` (`id`),
  CONSTRAINT `composer_era_ibfk_2` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer_era`
--

LOCK TABLES `composer_era` WRITE;
/*!40000 ALTER TABLE `composer_era` DISABLE KEYS */;
INSERT INTO `composer_era` VALUES (5,1),(3,2),(6,3),(4,4),(3,5),(6,6),(6,7),(4,8),(5,8),(5,9),(5,10),(6,11),(5,12),(6,13),(4,14),(5,15),(5,16),(6,17),(6,18),(5,19),(2,20),(5,21),(6,22),(3,23),(5,24),(5,25),(6,26),(5,27),(5,28),(5,29),(6,30),(5,31),(5,32),(6,33),(5,34),(4,35),(5,36),(5,37),(5,38),(3,39),(4,40),(6,41),(6,42),(4,43),(6,44),(6,45),(6,46),(6,47),(6,48),(6,49),(5,50),(5,51),(3,52),(5,53),(3,54),(5,55),(5,56),(5,57),(3,58),(4,59),(5,60),(6,61),(5,62),(3,63),(5,64),(6,65),(6,66),(6,67),(5,68),(3,69),(6,70),(6,71),(3,72),(6,73),(6,74),(6,75),(5,76),(6,77),(5,78),(5,79),(5,80),(6,81),(3,82),(5,83),(5,84),(6,85),(6,86),(6,87),(5,88),(5,89),(6,90),(6,91),(5,92),(3,93),(6,94),(5,95),(6,96),(3,97),(5,98),(6,99),(5,100);
/*!40000 ALTER TABLE `composer_era` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer_image`
--

DROP TABLE IF EXISTS `composer_image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer_image` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `composer_id` smallint(5) unsigned NOT NULL,
  `image` mediumblob NOT NULL,
  PRIMARY KEY (`id`),
  KEY `composer_id` (`composer_id`),
  CONSTRAINT `composer_image_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer_image`
--

LOCK TABLES `composer_image` WRITE;
/*!40000 ALTER TABLE `composer_image` DISABLE KEYS */;
/*!40000 ALTER TABLE `composer_image` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer_influence`
--

DROP TABLE IF EXISTS `composer_influence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer_influence` (
  `composer_id` smallint(5) unsigned NOT NULL,
  `influence_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`composer_id`,`influence_id`),
  KEY `influence_id` (`influence_id`),
  CONSTRAINT `composer_influence_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
  CONSTRAINT `composer_influence_ibfk_2` FOREIGN KEY (`influence_id`) REFERENCES `composer` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer_influence`
--

LOCK TABLES `composer_influence` WRITE;
/*!40000 ALTER TABLE `composer_influence` DISABLE KEYS */;
/*!40000 ALTER TABLE `composer_influence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer_link`
--

DROP TABLE IF EXISTS `composer_link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer_link` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `url` varchar(255) NOT NULL,
  `composer_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `url` (`url`),
  KEY `composer_id` (`composer_id`),
  CONSTRAINT `composer_link_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer_link`
--

LOCK TABLES `composer_link` WRITE;
/*!40000 ALTER TABLE `composer_link` DISABLE KEYS */;
/*!40000 ALTER TABLE `composer_link` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composer_nationality`
--

DROP TABLE IF EXISTS `composer_nationality`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composer_nationality` (
  `composer_id` smallint(5) unsigned NOT NULL,
  `nationality_id` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`composer_id`,`nationality_id`),
  KEY `nationality_id` (`nationality_id`),
  CONSTRAINT `composer_nationality_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
  CONSTRAINT `composer_nationality_ibfk_2` FOREIGN KEY (`nationality_id`) REFERENCES `nationality` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composer_nationality`
--

LOCK TABLES `composer_nationality` WRITE;
/*!40000 ALTER TABLE `composer_nationality` DISABLE KEYS */;
/*!40000 ALTER TABLE `composer_nationality` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composition`
--

DROP TABLE IF EXISTS `composition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composition` (
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `dates` varchar(50) NOT NULL,
  `nickname` varchar(50) DEFAULT NULL,
  `is_popular` tinyint(1) unsigned NOT NULL,
  `composition_collection_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `composition_collection_id` (`composition_collection_id`),
  CONSTRAINT `composition_ibfk_2` FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composition`
--

LOCK TABLES `composition` WRITE;
/*!40000 ALTER TABLE `composition` DISABLE KEYS */;
/*!40000 ALTER TABLE `composition` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composition_catalog`
--

DROP TABLE IF EXISTS `composition_catalog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composition_catalog` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `prefix` varchar(10) NOT NULL,
  `composer_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `composer_id` (`composer_id`),
  CONSTRAINT `composition_catalog_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composition_catalog`
--

LOCK TABLES `composition_catalog` WRITE;
/*!40000 ALTER TABLE `composition_catalog` DISABLE KEYS */;
/*!40000 ALTER TABLE `composition_catalog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composition_collection`
--

DROP TABLE IF EXISTS `composition_collection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composition_collection` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `is_popular` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composition_collection`
--

LOCK TABLES `composition_collection` WRITE;
/*!40000 ALTER TABLE `composition_collection` DISABLE KEYS */;
/*!40000 ALTER TABLE `composition_collection` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composition_collection_composer`
--

DROP TABLE IF EXISTS `composition_collection_composer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composition_collection_composer` (
  `composer_id` smallint(5) unsigned NOT NULL,
  `composition_collection_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`composer_id`,`composition_collection_id`),
  KEY `composition_collection_id` (`composition_collection_id`),
  CONSTRAINT `composition_collection_composer_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
  CONSTRAINT `composition_collection_composer_ibfk_2` FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composition_collection_composer`
--

LOCK TABLES `composition_collection_composer` WRITE;
/*!40000 ALTER TABLE `composition_collection_composer` DISABLE KEYS */;
/*!40000 ALTER TABLE `composition_collection_composer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `composition_composer`
--

DROP TABLE IF EXISTS `composition_composer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `composition_composer` (
  `composer_id` smallint(5) unsigned NOT NULL,
  `composition_id` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`composer_id`,`composition_id`),
  KEY `composition_id` (`composition_id`),
  CONSTRAINT `composition_composer_ibfk_1` FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
  CONSTRAINT `composition_composer_ibfk_2` FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `composition_composer`
--

LOCK TABLES `composition_composer` WRITE;
/*!40000 ALTER TABLE `composition_composer` DISABLE KEYS */;
/*!40000 ALTER TABLE `composition_composer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `era`
--

DROP TABLE IF EXISTS `era`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `era` (
  `id` tinyint(1) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(12) NOT NULL,
  `dates` varchar(9) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `era`
--

LOCK TABLES `era` WRITE;
/*!40000 ALTER TABLE `era` DISABLE KEYS */;
INSERT INTO `era` VALUES (1,'Medieval','0476/1400'),(2,'Renaissance','1400/1600'),(3,'Baroque','1600/1760'),(4,'Classical','1730/1820'),(5,'Romantic','1815/1910'),(6,'20th Century','1900/2000'),(7,'21st Century','2000/open');
/*!40000 ALTER TABLE `era` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `location`
--

DROP TABLE IF EXISTS `location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `location` (
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `location`
--

LOCK TABLES `location` WRITE;
/*!40000 ALTER TABLE `location` DISABLE KEYS */;
/*!40000 ALTER TABLE `location` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movement`
--

DROP TABLE IF EXISTS `movement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movement` (
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `number` tinyint(3) unsigned NOT NULL,
  `composition_id` mediumint(8) unsigned NOT NULL,
  `is_popular` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `composition_id` (`composition_id`),
  CONSTRAINT `movement_ibfk_1` FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movement`
--

LOCK TABLES `movement` WRITE;
/*!40000 ALTER TABLE `movement` DISABLE KEYS */;
/*!40000 ALTER TABLE `movement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nationality`
--

DROP TABLE IF EXISTS `nationality`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `nationality` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=238 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nationality`
--

LOCK TABLES `nationality` WRITE;
/*!40000 ALTER TABLE `nationality` DISABLE KEYS */;
INSERT INTO `nationality` VALUES (1,'Afghan'),(2,'Albanian'),(3,'Algerian'),(5,'American'),(4,'American Samoan'),(6,'Andorran'),(7,'Angolan'),(8,'Anguillian'),(9,'Antiguan'),(10,'Argentine'),(11,'Armenian'),(12,'Aruban'),(13,'Australian'),(14,'Austrian'),(15,'Azerbaijani'),(16,'Bahamian'),(17,'Bahraini'),(18,'Bangladeshi'),(19,'Barbadian'),(20,'Barbudan'),(21,'Basotho'),(22,'Belarusian'),(23,'Belgian'),(24,'Belizean'),(25,'Beninese'),(26,'Bermudian'),(27,'Bhutanese'),(28,'Bissau-Guinean'),(29,'Bolivian'),(30,'Bosnian'),(31,'Brazilian'),(33,'British'),(32,'British Virgin Islander'),(34,'Bruneian'),(35,'Bulgarian'),(36,'Burkinabe'),(37,'Burmese'),(38,'Burundian'),(39,'Cabo Verdean'),(40,'Cambodian'),(41,'Cameroonian'),(42,'Canadian'),(43,'Caymanian'),(44,'Central African'),(45,'Chadian'),(46,'Channel Islander (Guernsey)'),(47,'Channel Islander (Jersey)'),(48,'Chilean'),(51,'Chinese'),(49,'Chinese (Hong Kong)'),(50,'Chinese (Macau)'),(52,'Christmas Island'),(53,'Cocos Islander'),(54,'Colombian'),(55,'Comoran'),(56,'Congolese (Democratic Republic of the Congo)'),(57,'Congolese (Republic of the Congo)'),(58,'Cook Islander'),(59,'Costa Rican'),(60,'Croatian'),(61,'Cuban'),(62,'Curacaoan'),(63,'Cypriot'),(64,'Czech'),(65,'Danish'),(66,'Djiboutian'),(67,'Dominican (Dominica)'),(68,'Dominican (Dominican Republic)'),(69,'Dutch'),(70,'Ecuadorian'),(71,'Egyptian'),(72,'Emirati'),(73,'English'),(74,'Equatorial Guinean'),(75,'Eritrean'),(76,'Estonian'),(77,'Ethiopian'),(78,'Falkland Island'),(79,'Faroese'),(80,'Fijian'),(81,'Finnish'),(83,'French'),(82,'French Polynesian'),(84,'Futunan'),(85,'Gabonese'),(86,'Gambian'),(87,'Georgian'),(88,'German'),(89,'Ghanaian'),(90,'Gibraltarian'),(91,'Greek'),(92,'Greenlandic'),(93,'Grenadian'),(94,'Guamanian'),(95,'Guatemalan'),(96,'Guinean'),(97,'Guyanese'),(98,'Haitian'),(99,'Herzegovinian'),(100,'Honduran'),(101,'Hungarian'),(103,'I-Kiribati'),(102,'Icelandic'),(104,'Indian'),(105,'Indonesian'),(106,'Iranian'),(107,'Iraqi'),(108,'Irish'),(109,'Israeli'),(110,'Italian'),(111,'Ivorian'),(112,'Jamaican'),(113,'Japanese'),(114,'Jordanian'),(115,'Kazakhstani'),(116,'Kenyan'),(117,'Kittitian'),(118,'Kosovan'),(119,'Kuwaiti'),(120,'Kyrgyzstani'),(121,'Lao'),(122,'Latvian'),(123,'Lebanese'),(124,'Liberian'),(125,'Libyan'),(126,'Liechtenstein'),(127,'Lithuanian'),(128,'Luxembourg'),(129,'Macedonian'),(130,'Malagasy'),(131,'Malawian'),(132,'Malaysian'),(133,'Maldivian'),(134,'Malian'),(135,'Maltese'),(136,'Manx'),(137,'Marshallese'),(138,'Mauritanian'),(139,'Mauritian'),(140,'Mexican'),(141,'Micronesian'),(142,'Moldovan'),(143,'Monegasque'),(144,'Mongolian'),(145,'Montenegrin'),(146,'Montserratian'),(147,'Moroccan'),(148,'Motswana'),(149,'Mozambican'),(150,'Namibian'),(151,'Nauruan'),(152,'Nepali'),(153,'Nevisian'),(154,'New Caledonian'),(155,'New Zealand'),(160,'Ni-Vanuatu'),(156,'Nicaraguan'),(157,'Nigerian (Niger)'),(158,'Nigerian (Nigeria)'),(159,'Niuean'),(161,'Norfolk Islander'),(162,'North Korean'),(163,'Norwegian'),(164,'Omani'),(165,'Pakistani'),(166,'Palauan'),(167,'Palestinian'),(168,'Panamanian'),(169,'Papua New Guinean'),(170,'Paraguayan'),(171,'Peruvian'),(172,'Philippine'),(173,'Pitcairn Islander'),(174,'Polish'),(175,'Portuguese'),(176,'Puerto Rican'),(177,'Qatari'),(178,'Romanian'),(179,'Russian'),(180,'Rwandan'),(181,'Sahrawi'),(182,'Saint Helenian'),(183,'Saint Lucian'),(184,'Saint Vincentian'),(185,'Salvadoran'),(186,'Sammarinese'),(187,'Samoan'),(188,'Sao Tomean'),(189,'Saudi'),(190,'Scottish'),(191,'Senegalese'),(192,'Serbian'),(193,'Seychellois'),(194,'Sierra Leonean'),(195,'Singapore'),(196,'Slovak'),(197,'Slovenian'),(198,'Solomon Islander'),(199,'Somali'),(200,'South African'),(201,'South Korean'),(202,'South Sudanese'),(203,'Spanish'),(204,'Sri Lankan'),(205,'Sudanese'),(206,'Surinamese'),(207,'Swazi'),(208,'Swedish'),(209,'Swiss'),(210,'Syrian'),(211,'Taiwan'),(212,'Tajikistani'),(213,'Tanzanian'),(214,'Thai'),(215,'Timorese'),(216,'Tobagonian'),(217,'Togolese'),(218,'Tokelauan'),(219,'Tongan'),(220,'Trinidadian'),(221,'Tunisian'),(222,'Turkish'),(223,'Turkmen'),(224,'Tuvaluan'),(225,'Ugandan'),(226,'Ukrainian'),(227,'Unknown'),(228,'Uruguayan'),(229,'Uzbekistani'),(230,'Venezuelan'),(231,'Vietnamese'),(232,'Virgin Islander'),(233,'Wallisian'),(234,'Welsh'),(235,'Yemeni'),(236,'Zambian'),(237,'Zimbabwean');
/*!40000 ALTER TABLE `nationality` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `performer`
--

DROP TABLE IF EXISTS `performer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `performer` (
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `performer`
--

LOCK TABLES `performer` WRITE;
/*!40000 ALTER TABLE `performer` DISABLE KEYS */;
/*!40000 ALTER TABLE `performer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recording`
--

DROP TABLE IF EXISTS `recording`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recording` (
  `id` mediumint(8) unsigned NOT NULL,
  `dates` varchar(50) NOT NULL,
  `location_id` mediumint(8) unsigned DEFAULT NULL,
  `album_id` smallint(5) unsigned DEFAULT NULL,
  `track_number` tinyint(3) unsigned DEFAULT NULL,
  `composition_collection_id` smallint(5) unsigned NOT NULL,
  `composition_id` mediumint(8) unsigned NOT NULL,
  `movement_id` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `location_id` (`location_id`),
  KEY `album_id` (`album_id`),
  KEY `composition_collection_id` (`composition_collection_id`),
  KEY `composition_id` (`composition_id`),
  KEY `movement_id` (`movement_id`),
  CONSTRAINT `recording_ibfk_1` FOREIGN KEY (`location_id`) REFERENCES `location` (`id`),
  CONSTRAINT `recording_ibfk_2` FOREIGN KEY (`album_id`) REFERENCES `album` (`id`),
  CONSTRAINT `recording_ibfk_3` FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`),
  CONSTRAINT `recording_ibfk_4` FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`),
  CONSTRAINT `recording_ibfk_5` FOREIGN KEY (`movement_id`) REFERENCES `movement` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recording`
--

LOCK TABLES `recording` WRITE;
/*!40000 ALTER TABLE `recording` DISABLE KEYS */;
/*!40000 ALTER TABLE `recording` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recording_performer`
--

DROP TABLE IF EXISTS `recording_performer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recording_performer` (
  `recording_id` mediumint(8) unsigned NOT NULL,
  `performer_id` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`recording_id`,`performer_id`),
  KEY `performer_id` (`performer_id`),
  CONSTRAINT `recording_performer_ibfk_1` FOREIGN KEY (`performer_id`) REFERENCES `performer` (`id`),
  CONSTRAINT `recording_performer_ibfk_2` FOREIGN KEY (`recording_id`) REFERENCES `recording` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recording_performer`
--

LOCK TABLES `recording_performer` WRITE;
/*!40000 ALTER TABLE `recording_performer` DISABLE KEYS */;
/*!40000 ALTER TABLE `recording_performer` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-07-03 16:16:03
