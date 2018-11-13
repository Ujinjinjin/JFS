CREATE TABLE CommonField (
    id   INTEGER CONSTRAINT pk_common_field PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    verbose TEXT NULL
);

CREATE TABLE JiraField (
    id   INTEGER CONSTRAINT pk_jira_field PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    verbose TEXT NOT NULL
);

CREATE TABLE Profile (
    id   INTEGER CONSTRAINT pk_profile PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    active BOOLEAN NOT NULL
);

CREATE TABLE Sync (
    id      INTEGER CONSTRAINT pk_sync PRIMARY KEY AUTOINCREMENT,
    jira_id INTEGER UNIQUE,
    tfs_id  INTEGER UNIQUE,
    rev INTEGER
);

CREATE TABLE TfsField (
    id   INTEGER CONSTRAINT pk_tfs_field PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    verbose TEXT NOT NULL
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

CREATE TABLE WorkItemType (
    id   INTEGER CONSTRAINT pk_work_item_type PRIMARY KEY AUTOINCREMENT,
    type TEXT UNIQUE NOT NULL
);

CREATE TABLE Config (
    id                INTEGER CONSTRAINT pk_config PRIMARY KEY AUTOINCREMENT,
    profile_id        INTEGER NOT NULL REFERENCES Profile (id),
    priority          INTEGER,
    sprint            TEXT    NOT NULL,
    work_item_type_id INTEGER NOT NULL REFERENCES WorkItemType (id),
    UNIQUE (work_item_type_id, priority)
        ON CONFLICT FAIL
);

CREATE INDEX idx_config__profile
    ON Config (profile_id);

CREATE INDEX idx_config__work_item_type
    ON Config (work_item_type_id)