CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_team`(
	team_name varchar(50),
    out team_id int
)
begin
	insert into teams (team_name)
    values(team_name);
    
    select count(*)
    into team_id
    from teams;
end