insert into CommonField (name)
values ('Title'),
       ('Type'),
       ('Priority'),
       ('Sprint'),
       ('Description'),
       ('Team'),
       ('Project'),
       ('Link'),
       ('Relation Link'),
       ('Attachment'),
       ('Parent'),
       ('Bug'),
       ('Task'),
       ('Story');

insert into TfsField (name, verbose)
values ('/fields/System.Title', 'Title'),
       ('/fields/Microsoft.VSTS.Common.Priority', 'Priority'),
       ('/fields/System.IterationPath', 'Sprint'),
       ('/fields/System.Description', 'Description'),
       ('/fields/System.TeamProject', 'Project'),
       ('/fields/System.AreaPath', 'Area Path'),
       ('/relations/-', 'Relation Link'),
       ('System.LinkTypes.Hierarchy-Reverse', 'Parent'),
       ('bug', 'Bug'),
       ('task', 'Task'),
       ('story', 'Story');