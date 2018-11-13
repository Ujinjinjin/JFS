insert into Profile (name, active)
values ('Default Profile', true);

insert into TfsConfig (priority, parent_id, sprint, area, team_project)
values (1, 2, 'JFS\\Iteration 1', 'JFS', 'JFS');

insert into JiraConfig (priority, sprint)
values ('Minor', 'Nice');

insert into Config (tfs_config_id, jira_config_id, profile_id)
values (1, 1, 1);