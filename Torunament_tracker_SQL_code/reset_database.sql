CREATE DEFINER=`root`@`localhost` PROCEDURE `reset_database`()
BEGIN
delete from matchup_entries;
delete from matchups;
delete from prizes;
delete from participants;
delete from team_members;
delete from teams;
delete from tournament_entries;
delete from tournament_prizes;
delete from tournaments;

ALTER TABLE matchup_entries AUTO_INCREMENT = 1;
ALTER TABLE matchups AUTO_INCREMENT = 1;
ALTER TABLE participants AUTO_INCREMENT = 1;
ALTER TABLE prizes AUTO_INCREMENT = 1;
ALTER TABLE team_members AUTO_INCREMENT = 1;
ALTER TABLE teams AUTO_INCREMENT = 1;
ALTER TABLE tournament_entries AUTO_INCREMENT = 1;
ALTER TABLE tournament_prizes AUTO_INCREMENT = 1;
ALTER TABLE tournaments AUTO_INCREMENT = 1;
END