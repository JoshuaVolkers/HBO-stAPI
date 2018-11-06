-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema elbho_register_prod
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema elbho_register_prod
-- -----------------------------------------------------
USE `elbho_register_prod`;

-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_aanvragen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_aanvragen` (
  `aanvraag_id` INT(11) NOT NULL AUTO_INCREMENT,
  `aanvraag_nummer` TEXT NOT NULL,
  `aanvraag_bedrijf` TEXT NOT NULL,
  `aanvraag_adres` TEXT NOT NULL,
  `aanvraag_contactpersoon_naam` TEXT NOT NULL,
  `aanvraag_contactpersoon_telefoon` TEXT NOT NULL,
  `aanvraag_contactpersoon_email` TEXT NOT NULL,
  `aanvraag_contactpersoon_functie` TEXT NOT NULL,
  `aanvraag_opleidingen` TEXT NOT NULL COMMENT 'indien bedrijf al voorkeur heeft voor opleidingen, hier invullen',
  `aanvraag_fte` INT(11) NOT NULL,
  `aanvraag_begeleiders` INT(11) NOT NULL,
  `aanvraag_stageplaatsen` INT(11) NOT NULL,
  `aanvraag_status` INT(11) NOT NULL DEFAULT '0' COMMENT '0 = ingediend, 1 = in behandeling (vooronderzoek gestart), 2 = vooronderzoek afgerond, 3 = in behandeling (bedrijfsbezoek/belafspraak gepland, 4 = bedrijfsbezoek uitgevoerd 5 = afgerond',
  `aanvraag_door` INT(11) NOT NULL DEFAULT '0' COMMENT '0 = ELBHO, 1 = bedrijf zelf, 2 = onderwijsinstelling',
  `aanvraag_door_id` INT(11) NOT NULL COMMENT 'indien onderwijsinstelling: naam van stagecoordinator',
  `aanvraag_voordracht` INT(1) NOT NULL DEFAULT '0' COMMENT '0 = regulier, 1 = verkort traject (voordracht) - telefonisch',
  `aanvraag_validator` INT(11) NOT NULL,
  `aanvraag_datum` DATE NOT NULL COMMENT 'datum waarop aanvraag is gedaan',
  `aanvraag_update_datum` DATE NOT NULL COMMENT 'datum wanneer laatste update heeft plaatsgevonden',
  `aanvraag_agent` TEXT NOT NULL COMMENT 'naam van persoon die de aanvraag heeft binnengebracht',
  `aanvraag_pilotcode` TEXT NOT NULL,
  `aanvraag_tarief_eenmalig` FLOAT NOT NULL,
  `aanvraag_tarief_jaarlijks_eerstejaar` FLOAT NOT NULL,
  `aanvraag_tarief_jaarlijks` FLOAT NOT NULL,
  `aanvraag_akkoord_email` INT(1) NOT NULL DEFAULT '0' COMMENT 'is klant zelf akkoord gegaan via bevestiging knop',
  `aanvraag_afgerond` INT(1) NOT NULL DEFAULT '0' COMMENT '0 = nog niet afgerond, 1 = afgerond',
  `aanvraag_actief` INT(11) NOT NULL DEFAULT '1' COMMENT '0 = niet actief, 1 = actief',
  PRIMARY KEY (`aanvraag_id`),
  INDEX `aanvraag_id` (`aanvraag_id` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_aanvragen_historie`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_aanvragen_historie` (
  `ahis_id` INT(11) NOT NULL AUTO_INCREMENT,
  `ahis_aanvraag_id` INT(11) NOT NULL,
  `ahis_datum` DATE NOT NULL,
  `ahis_user` INT(11) NOT NULL,
  `ahis_omschrijving` TEXT NOT NULL,
  PRIMARY KEY (`ahis_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_bedrijven`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_bedrijven` (
  `bedrijf_id` INT(11) NOT NULL AUTO_INCREMENT,
  `bedrijf_kvk` INT(11) NOT NULL,
  `bedrijf_handelsnaam` TEXT NOT NULL,
  `bedrijf_vestiging_straat` TEXT NOT NULL,
  `bedrijf_vestiging_huisnr` TEXT NOT NULL,
  `bedrijf_vestiging_toev` TEXT NOT NULL,
  `bedrijf_vestiging_postcode` TEXT NOT NULL,
  `bedrijf_vestiging_plaats` TEXT NOT NULL,
  `bedrijf_vestiging_land` INT(11) NOT NULL,
  `bedrijf_contactpersoon_naam` TEXT NOT NULL,
  `bedrijf_contactpersoon_functie` TEXT NOT NULL,
  `bedrijf_contactpersoon_email` TEXT NOT NULL,
  `bedrijf_contactpersoon_telefoon` TEXT NOT NULL,
  `bedrijf_website` TEXT NOT NULL,
  `bedrijf_logo` TEXT NOT NULL,
  `bedrijf_reg_datum` DATE NOT NULL,
  `bedrijf_reg_door` INT(11) NOT NULL,
  `bedrijf_gecontroleerd_door` INT(11) NOT NULL COMMENT 'verwijst naar reg_medewerkers',
  `bedrijf_gecontroleerd_datum` DATE NOT NULL,
  `bedrijf_erkend_stagebedrijf` INT(1) NOT NULL DEFAULT '0',
  `bedrijf_erkend_afstudeerbedrijf` INT(1) NOT NULL DEFAULT '0',
  `bedrijf_plaatsen_stage` INT(11) NOT NULL COMMENT 'hoeveel stageplaatsen beschikbaar?',
  `bedrijf_plaatsen_afstuderen` INT(11) NOT NULL COMMENT 'hoeveel afstudeerplaatsen beschikbaar?',
  `bedrijf_begeleiders` INT(11) NOT NULL COMMENT 'hoeveel begeleiders beschikbaar?',
  `bedrijf_medewerkers` INT(11) NOT NULL COMMENT 'hoeveel medewerkers heeft het bedrijf in dienst ? in aantalllen, niet in FTE',
  `bedrijf_zwartelijst` INT(11) NOT NULL DEFAULT '0',
  `bedrijf_jaarbedrag` FLOAT NOT NULL DEFAULT '350' COMMENT 'jaarbedrag in euro\'\'s en met punten als decimaalteken',
  `bedrijf_bedrag_eenmalig` FLOAT NOT NULL DEFAULT '0',
  `bedrijf_actief` INT(1) NOT NULL DEFAULT '1',
  `bedrijf_social_linkedin` TEXT NOT NULL,
  `bedrijf_beschrijving` TEXT NOT NULL,
  `bedrijf_breedtegraad` DECIMAL(5,2) NOT NULL,
  `bedrijf_lengtegraad` DECIMAL(5,2) NOT NULL,
  PRIMARY KEY (`bedrijf_id`),
  INDEX `bedrijf_id` (`bedrijf_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_erkenning`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_erkenning` (
  `erkenning_id` INT(11) NOT NULL AUTO_INCREMENT,
  `erkenning_bedrijf_id` INT(11) NOT NULL,
  `erkenning_erkend` INT(11) NOT NULL DEFAULT '1' COMMENT '0 = niet erkend, 1 = erkend, 2 = onder review',
  `erkenning_type` INT(11) NOT NULL COMMENT '0 = stagebedrijf, 1 = afstudeerbedrijf, 2 = beide',
  `erkenning_datum_bezoek` DATE NOT NULL,
  `erkenning_datum_begin` DATE NOT NULL,
  `erkenning_datum_eind` DATE NOT NULL,
  `erkenning_validator` INT(11) NOT NULL COMMENT 'verwijst naar reg_users',
  `erkenning_rapportage` TEXT NOT NULL COMMENT 'verwijst naar ingevulde rapportage op de server',
  `erkenning_actief` INT(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`erkenning_id`),
  INDEX `erkenning_id` (`erkenning_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_facturen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_facturen` (
  `factuur_id` INT(11) NOT NULL AUTO_INCREMENT,
  `factuur_bedrijf_id` INT(11) NOT NULL,
  `factuur_totaal_incl` FLOAT(10,2) NOT NULL,
  `factuur_betaald` INT(1) NOT NULL DEFAULT '0',
  `factuur_actief` INT(1) NOT NULL DEFAULT '1',
  INDEX `factuur_id` (`factuur_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_landen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_landen` (
  `land_id` INT(11) NOT NULL AUTO_INCREMENT,
  `land_naam` TEXT NOT NULL,
  PRIMARY KEY (`land_id`),
  INDEX `land_id` (`land_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_onderwijsinstellingen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_onderwijsinstellingen` (
  `instelling_id` INT(11) NOT NULL AUTO_INCREMENT,
  `instelling_brin` TEXT NOT NULL,
  `instelling_naam` TEXT NOT NULL,
  `instelling_plaats` TEXT NOT NULL,
  `instelling_actief` INT(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`instelling_id`),
  INDEX `instelling_id` (`instelling_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_opleiding_per_bedrijf`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_opleiding_per_bedrijf` (
  `opb_id` INT(11) NOT NULL AUTO_INCREMENT,
  `opb_bedrijf_id` INT(11) NOT NULL,
  `opb_opleiding_id` INT(11) NOT NULL,
  `opb_niveau` INT(11) NOT NULL COMMENT '0 = stage, 1 = afstuderen, 2 = beide',
  `opb_datum_begin` DATE NOT NULL,
  `opb_datum_eind` DATE NOT NULL,
  `opb_actief` INT(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`opb_id`),
  INDEX `opb_id` (`opb_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_opleidingen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_opleidingen` (
  `opl_id` INT(11) NOT NULL AUTO_INCREMENT,
  `opl_croho_nr` TEXT NOT NULL,
  `opl_naam` TEXT NOT NULL,
  `opl_niveau` TEXT NOT NULL,
  `opl_actief` INT(11) NOT NULL,
  PRIMARY KEY (`opl_id`),
  INDEX `opl_id` (`opl_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_opleidingsniveau`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_opleidingsniveau` (
  `opn_id` INT(11) NOT NULL AUTO_INCREMENT,
  `opn_naam` TEXT NOT NULL,
  PRIMARY KEY (`opn_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_bin;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_potentials`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_potentials` (
  `potential_id` INT(11) NOT NULL AUTO_INCREMENT,
  `potential_naam` TEXT NOT NULL,
  `potential_contactpersoon` TEXT NOT NULL,
  `potential_email` TEXT NOT NULL,
  `potential_telefoon` TEXT NOT NULL,
  `potential_status` INT(1) NOT NULL DEFAULT '0' COMMENT '0 = open, 1 = afgerond (succesvol), 2 = afgerond (helaas), 3 = terugbellen',
  `potential_terugbellen` DATE NOT NULL,
  PRIMARY KEY (`potential_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_reviews`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_reviews` (
  `review_id` INT(11) NOT NULL AUTO_INCREMENT,
  `review_bedrijf_id` INT(11) NOT NULL,
  `review_student_id` INT(11) NOT NULL,
  `review_sterren` INT(11) NOT NULL,
  `review_geschreven` TEXT NOT NULL,
  `review_anoniem` INT(1) NOT NULL DEFAULT '0' COMMENT '0 = anoniem, 1 = niet anoniem',
  `review_datum` DATETIME NOT NULL,
  `review_status` INT(11) NOT NULL DEFAULT '0' COMMENT '0 = in afwachting, 1 = bevestigd, 2 = afgewezen',
  `review_status_bevestigd_door` INT(11) NOT NULL,
  `review_stagecontract` BLOB NULL DEFAULT NULL COMMENT 'PDF file',
  PRIMARY KEY (`review_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 33
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_stagesoort`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_stagesoort` (
  `stage_id` INT(32) NOT NULL AUTO_INCREMENT,
  `stagesoort` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`stage_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_student_email`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_student_email` (
  `se_id` INT(11) NOT NULL AUTO_INCREMENT,
  `se_domein` TEXT NOT NULL,
  `se_onderwijsinstelling_id` INT(11) NOT NULL,
  PRIMARY KEY (`se_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_talen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_talen` (
  `talen_id` INT(11) NOT NULL AUTO_INCREMENT,
  `talen_naam` TEXT NOT NULL,
  `talen_iso` TEXT NOT NULL,
  PRIMARY KEY (`talen_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_user_studenten`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_user_studenten` (
  `user_id` INT(11) NOT NULL COMMENT 'Is hetzelfde als de id van de student in de users tabel',
  `user_land` INT(11) NOT NULL,
  `user_woonplaats` TEXT NOT NULL,
  `user_opleiding_id` INT(11) NOT NULL,
  `user_op_niveau` INT(11) NOT NULL,
  `user_taal` INT(11) NOT NULL,
  `user_social_account` INT(1) NOT NULL DEFAULT '0' COMMENT '0 = user is niet aangemaakt via een social media account, 1 = user is aangemaakt via een social media account',
  `user_breedtegraad` DECIMAL(5,2) NOT NULL,
  `user_lengtegraad` DECIMAL(5,2) NOT NULL,
  PRIMARY KEY (`user_id`),
  INDEX `user_id` (`user_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_user_verification`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_user_verification` (
  `ver_id` INT(11) NOT NULL AUTO_INCREMENT,
  `ver_user_id` INT(11) NOT NULL,
  `ver_token` TEXT NOT NULL,
  `ver_expiration` DATETIME NOT NULL,
  PRIMARY KEY (`ver_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 12
DEFAULT CHARACTER SET = latin1
ROW_FORMAT = COMPACT;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_users` (
  `user_id` INT(11) NOT NULL AUTO_INCREMENT,
  `user_email` TEXT NOT NULL,
  `user_password` TEXT NULL DEFAULT NULL,
  `user_name` TEXT NOT NULL,
  `user_role` INT(11) NOT NULL COMMENT '0 = student, 1 = bedrijf, 2 = onderwijs, 3 = validator, 4 = elbho medewerker',
  `user_role_id` INT(11) NOT NULL COMMENT 'verwijst naar reg_bedrijven als dit een bedrijfsmedewerker is of naar reg_onderwijsinstellingen als het een coordinator is',
  `user_active` INT(1) NOT NULL DEFAULT '1',
  `user_account_type` INT(1) NOT NULL DEFAULT '0',
  `user_refresh_token` VARCHAR(32) NULL DEFAULT NULL COMMENT 'Unique refreshtoken used for retrieving new accesstoken for the API.',
  PRIMARY KEY (`user_id`),
  INDEX `user_id` (`user_id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 33
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_vacatures`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_vacatures` (
  `vacature_id` INT(11) NOT NULL AUTO_INCREMENT,
  `vacature_bedrijf_id` INT(11) NOT NULL,
  `vacature_user_id` INT(11) NOT NULL COMMENT 'welke user heeft dit toegevoegd?',
  `vacature_titel` TEXT NOT NULL,
  `vacature_plaats` TEXT NOT NULL,
  `vacature_datum_plaatsing` DATE NOT NULL,
  `vacature_datum_verlopen` DATE NOT NULL COMMENT 'tot wanneer mag er gesolliciteerd worden?',
  `vacature_tekst` TEXT NOT NULL,
  `vacature_link` TEXT NOT NULL,
  `vacature_actief` INT(1) NOT NULL DEFAULT '1' COMMENT '0 = niet actief, 1 = actief',
  `vacature_op_niveau` INT(11) NOT NULL,
  `vacature_taal` INT(11) NOT NULL,
  `vacature_breedtegraad` DECIMAL(5,2) NOT NULL,
  `vacature_lengtegraad` DECIMAL(5,2) NOT NULL,
  `vacature_stagesoort` INT(1) NOT NULL,
  PRIMARY KEY (`vacature_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_vacatures_favoriet`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_vacatures_favoriet` (
  `vf_id` INT(11) NOT NULL AUTO_INCREMENT,
  `vf_vacature_id` INT(11) NOT NULL,
  `vf_user_id` INT(11) NOT NULL,
  PRIMARY KEY (`vf_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 18
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_bin;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_vacatures_opleidingen`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_vacatures_opleidingen` (
  `rvo_id` INT(11) NOT NULL AUTO_INCREMENT,
  `rvo_vacature_id` INT(11) NOT NULL,
  `rvo_opleiding_id` INT(11) NOT NULL,
  PRIMARY KEY (`rvo_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_vacatures_stagesoort`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_vacatures_stagesoort` (
  `rvo_id` INT(11) NOT NULL AUTO_INCREMENT,
  `rvs_vacature_id` INT(11) NOT NULL,
  `rvs_stagesoort_id` INT(11) NOT NULL,
  PRIMARY KEY (`rvo_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `elbho_register_prod`.`reg_zoekhistorie`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elbho_register_prod`.`reg_zoekhistorie` (
  `historie_id` INT(11) NOT NULL AUTO_INCREMENT,
  `historie_bedrijf_id` INT(11) NOT NULL,
  `historie_datum` DATE NOT NULL,
  `historie_tijd` TIME NOT NULL,
  `historie_user_id` INT(11) NOT NULL COMMENT 'indien deze 0 is, dan wordt het IP opgeslagen',
  `historie_ip` TEXT NOT NULL,
  PRIMARY KEY (`historie_id`),
  INDEX `historie_id` (`historie_id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
