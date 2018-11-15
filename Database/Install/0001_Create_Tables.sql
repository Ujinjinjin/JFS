CREATE TABLE CommonField (
    id      INTEGER CONSTRAINT pk_common_field PRIMARY KEY AUTOINCREMENT,
    name    TEXT UNIQUE NOT NULL,
    verbose TEXT        NULL
);

CREATE TABLE JiraField (
    id      INTEGER CONSTRAINT pk_jira_field PRIMARY KEY AUTOINCREMENT,
    name    TEXT UNIQUE NOT NULL,
    verbose TEXT        NOT NULL
);

CREATE TABLE Profile (
    id     INTEGER CONSTRAINT pk_profile PRIMARY KEY AUTOINCREMENT,
    name   TEXT UNIQUE NOT NULL,
    active BOOLEAN     NOT NULL
);

CREATE TABLE Sync (
    id       INTEGER CONSTRAINT pk_sync PRIMARY KEY AUTOINCREMENT,
    jira_key TEXT    NOT NULL,
    tfs_id   INTEGER NOT NULL,
    rev      INTEGER NOT NULL,
    deleted  BOOLEAN                                DEFAULT FALSE,
    UNIQUE (jira_key, tfs_id)
        ON CONFLICT FAIL
);

CREATE TABLE TfsField (
    id      INTEGER CONSTRAINT pk_tfs_field PRIMARY KEY AUTOINCREMENT,
    name    TEXT UNIQUE NOT NULL,
    verbose TEXT        NOT NULL
);

CREATE TABLE Mapping (
    id              INTEGER CONSTRAINT pk_mapping PRIMARY KEY AUTOINCREMENT,
    profile_id      INTEGER NOT NULL REFERENCES Profile (id),
    common_field_id INTEGER NOT NULL REFERENCES CommonField (id),
    tfs_field_id    INTEGER NOT NULL REFERENCES TfsField (id),
    jira_field_id   INTEGER NOT NULL REFERENCES JiraField (id),
    UNIQUE (profile_id, common_field_id, tfs_field_id, jira_field_id)
        ON CONFLICT FAIL
);

CREATE INDEX idx_mapping__common_field
    ON Mapping (common_field_id);

CREATE INDEX idx_mapping__jira_field
    ON Mapping (jira_field_id);

CREATE INDEX idx_mapping__profile
    ON Mapping (profile_id);

CREATE INDEX idx_mapping__tfs_fields
    ON Mapping (tfs_field_id);

CREATE TABLE TfsConfig (
    id           INTEGER CONSTRAINT pk_tfs_config PRIMARY KEY AUTOINCREMENT,
    priority     INTEGER NOT NULL,
    parent_id    INTEGER NOT NULL,
    sprint       TEXT    NOT NULL,
    area         TEXT    NOT NULL,
    team_project TEXT    NOT NULL
);

CREATE TABLE JiraConfig (
    id       INTEGER CONSTRAINT pk_jira_config PRIMARY KEY AUTOINCREMENT,
    priority TEXT NOT NULL,
    sprint   TEXT NOT NULL,
    project  TEXT NOT NULL
);

CREATE TABLE Config (
    id             INTEGER CONSTRAINT pk_config PRIMARY KEY AUTOINCREMENT,
    tfs_config_id  INTEGER        NOT NULL REFERENCES TfsConfig (id),
    jira_config_id INTEGER        NOT NULL REFERENCES JiraConfig (id),
    profile_id     INTEGER UNIQUE NOT NULL REFERENCES Profile (id)
);

CREATE INDEX idx_config__jira_config
    ON config (jira_config_id);

CREATE INDEX idx_config__profile
    ON config (profile_id);

CREATE INDEX idx_config__tfs_config
    ON config (tfs_config_id);