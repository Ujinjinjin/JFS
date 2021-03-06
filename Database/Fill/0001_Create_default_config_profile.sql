insert into Profile (name, active)
values ('Default Profile', 1);

insert into TfsConfig (priority, parent_id, sprint, area, team_project)
values (1, 2, 'JFS\\Iteration 1', 'JFS', 'JFS'),
       (2, 2, 'JFS\\Iteration 1', 'JFS', 'JFS'),
       (3, 2, 'JFS\\Iteration 1', 'JFS', 'JFS');

insert into JiraConfig (priority, sprint, project)
values ('Highest', 'Nice', 'JFS'),
       ('High', 'Nice', 'JFS'),
       ('Medium', 'Nice', 'JFS');

insert into Config (tfs_config_id, jira_config_id, profile_id)
values (1, 1, 1),
       (2, 2, 1),
       (3, 3, 1);

insert into Priority (tfs_priority, jira_priority)
values (1, 'Highest'),
       (2, 'High'),
       (3, 'Medium'),
       (4, 'Low'),
       (5, 'Lowest');