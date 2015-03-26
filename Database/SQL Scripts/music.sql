CREATE TABLE `performer` (
    `id` MEDIUMINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `album` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `location` (
    `id` MEDIUMINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY (`id`)
);

CREATE TABLE `era` (
    `id` TINYINT(1) UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(12) NOT NULL UNIQUE,
    `dates` VARCHAR(9) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `nationality` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY (`id`)
);

CREATE TABLE `composer` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `dates` VARCHAR(50) NOT NULL,
    `birth_location_id` MEDIUMINT UNSIGNED,
    `death_location_id` MEDIUMINT UNSIGNED,
    `biography` TEXT,
    `is_popular` TINYINT(1) NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`birth_location_id`) REFERENCES `location` (`id`),
    FOREIGN KEY (`death_location_id`) REFERENCES `location` (`id`)
);

CREATE TABLE `composition_collection` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `is_popular` TINYINT(1) UNSIGNED NOT NULL,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composition_catalog` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `prefix` VARCHAR(10) NOT NULL,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composer_influence` (
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    `influence_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`composer_id`,`influence_id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
    FOREIGN KEY (`influence_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composer_image` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `path` VARCHAR(255) NOT NULL UNIQUE,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
	PRIMARY KEY (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composer_link` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `url` VARCHAR(255) NOT NULL UNIQUE,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composer_era` (
    `era_id` TINYINT(1) UNSIGNED NOT NULL,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`era_id`,`composer_id`),
    FOREIGN KEY (`era_id`) REFERENCES `era` (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`)
);

CREATE TABLE `composer_nationality` (
	`composer_id` SMALLINT UNSIGNED NOT NULL,
    `nationality_id` SMALLINT UNSIGNED,
    PRIMARY KEY (`composer_id`,`nationality_id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
    FOREIGN KEY (`nationality_id`) REFERENCES `nationality` (`id`)
);

CREATE TABLE `composition` (
    `id` MEDIUMINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(100) NOT NULL,
    `dates` VARCHAR(50) NOT NULL,
    `nickname` VARCHAR(50),
    `is_popular` TINYINT(1) UNSIGNED NOT NULL,
    `composer_id` SMALLINT UNSIGNED NOT NULL,
    `composition_collection_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
    FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`)
);

CREATE TABLE `composition_collection_composer` (
	`composer_id` SMALLINT UNSIGNED NOT NULL,
    `composition_collection_id` SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (`composer_id`,`composition_collection_id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
    FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`)
);

CREATE TABLE `movement` (
    `id` MEDIUMINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `number` TINYINT UNSIGNED NOT NULL,
    `composition_id` MEDIUMINT UNSIGNED NOT NULL,
    `is_popular` TINYINT(1) UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
);

CREATE TABLE `catalog_number` (
    `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `number` VARCHAR(15) NOT NULL,
    `composition_catalog_id` SMALLINT UNSIGNED NOT NULL,
    `composition_collection_id` SMALLINT UNSIGNED NOT NULL,
    `composition_id` MEDIUMINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`composition_catalog_id`) REFERENCES `composition_catalog` (`id`),    
    FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`),
    FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
);

CREATE TABLE `composition_composer` (
	`composer_id` SMALLINT UNSIGNED NOT NULL,
    `composition_id` MEDIUMINT UNSIGNED NOT NULL,
    PRIMARY KEY (`composer_id`,`composition_id`),
    FOREIGN KEY (`composer_id`) REFERENCES `composer` (`id`),
    FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`)
);

CREATE TABLE `recording` (
    `id` MEDIUMINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `dates` VARCHAR(50) NOT NULL,
    `location_id` MEDIUMINT UNSIGNED,
    `album_id` SMALLINT UNSIGNED,
    `track_number` TINYINT UNSIGNED,
    `composition_collection_id` SMALLINT UNSIGNED NOT NULL,
    `composition_id` MEDIUMINT UNSIGNED NOT NULL,
    `movement_id` MEDIUMINT UNSIGNED NOT NULL,
    PRIMARY KEY (`id`),
    FOREIGN KEY (`location_id`) REFERENCES `location` (`id`),
    FOREIGN KEY (`album_id`) REFERENCES `album` (`id`),
    FOREIGN KEY (`composition_collection_id`) REFERENCES `composition_collection` (`id`),
    FOREIGN KEY (`composition_id`) REFERENCES `composition` (`id`),
    FOREIGN KEY (`movement_id`) REFERENCES `movement` (`id`)
);

CREATE TABLE `recording_performer` (
    `recording_id` MEDIUMINT UNSIGNED NOT NULL,
	`performer_id` MEDIUMINT UNSIGNED NOT NULL,
    PRIMARY KEY (`recording_id`, `performer_id`),
    FOREIGN KEY (`performer_id`) REFERENCES `performer` (`id`),
    FOREIGN KEY (`recording_id`)  REFERENCES `recording` (`id`)
);