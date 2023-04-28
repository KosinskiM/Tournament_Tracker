CREATE DEFINER=`root`@`localhost` PROCEDURE `get_teamsByWinnerId`(
	winner_id tinyint,
    out TeamName varchar(255)
)
BEGIN
select 
    team_name
    into TeamName
from teams
where team_id = winner_id;
END