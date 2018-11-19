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
    -- Temporary fields, till the better days
    title    TEXT    NOT NULL,
    descr    TEXT,
    priority INTEGER,

    UNIQUE (jira_key, tfs_id)
        ON CONFLICT FAIL
);

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
    tfs_config_id  INTEGER NOT NULL REFERENCES TfsConfig (id),
    jira_config_id INTEGER NOT NULL REFERENCES JiraConfig (id),
    profile_id     INTEGER NOT NULL REFERENCES Profile (id)
);

CREATE INDEX idx_config__jira_config
    ON config (jira_config_id);

CREATE INDEX idx_config__profile
    ON config (profile_id);

CREATE INDEX idx_config__tfs_config
    ON config (tfs_config_id);

CREATE TABLE Priority (
    id            INTEGER CONSTRAINT pk_priority PRIMARY KEY AUTOINCREMENT,
    tfs_priority  INTEGER UNIQUE,
    jira_priority TEXT UNIQUE
);