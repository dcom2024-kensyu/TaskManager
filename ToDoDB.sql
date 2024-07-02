CREATE TABLE todos (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    is_completed BIT NOT NULL DEFAULT 0,
    priority INT NOT NULL DEFAULT 0,
    created_at DATETIME2,
	created_by BIGINT,
	updated_at DATETIME2,
	updated_by BIGINT,
    due_date DATETIME2,
);

CREATE TABLE users (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    user_name NVARCHAR(255) NOT NULL,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    created_at DATETIME2,
	created_by BIGINT,
	updated_at DATETIME2,
	updated_by BIGINT,
);

CREATE TABLE categories (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
);

CREATE TABLE todo_categories (
    todo_id BIGINT NOT NULL,
    category_id BIGINT NOT NULL,
    PRIMARY KEY (todo_id, category_id),
    FOREIGN KEY (todo_id) REFERENCES todos(id),
    FOREIGN KEY (category_id) REFERENCES categories(id),
);

CREATE TABLE comments (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    todo_id BIGINT NOT NULL,
    user_id BIGINT NOT NULL,
    comment NVARCHAR(MAX) NOT NULL,
    created_at DATETIME2,
	created_by BIGINT,
	updated_at DATETIME2,
	updated_by BIGINT,
    FOREIGN KEY (todo_id) REFERENCES todos(id),
    FOREIGN KEY (user_id) REFERENCES users(id),
);

CREATE TABLE reminders (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    todo_id BIGINT NOT NULL,
    reminder_date DATETIME2 NOT NULL,
    is_sent BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (todo_id) REFERENCES todos(id),
);

CREATE TABLE user_roles (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    role_name NVARCHAR(255) NOT NULL UNIQUE,
);

CREATE TABLE user_roles_assignments (
    user_id BIGINT NOT NULL,
    role_id BIGINT NOT NULL,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (role_id) REFERENCES user_roles(id),
);

INSERT INTO users (user_name, password, email) VALUES ('user1', 'password1', 'user1@example.com');
INSERT INTO users (user_name, password, email) VALUES ('user2', 'password2', 'user2@example.com');

INSERT INTO todos (title, description, is_completed, priority) VALUES ('task Item 1', 'Description for item 1', 0, 1);
INSERT INTO todos (title, description, is_completed, priority) VALUES ('task Item 2', 'Description for item 2', 0, 2);

INSERT INTO categories (name, description) VALUES ('Work', 'Tasks related to work');
INSERT INTO categories (name, description) VALUES ('Personal', 'Personal tasks');

INSERT INTO todo_categories (todo_id, category_id) VALUES (1, 1);
INSERT INTO todo_categories (todo_id, category_id) VALUES (2, 2);

INSERT INTO comments (todo_id, user_id, comment) VALUES (1, 1, 'This is a comment on item 1');
INSERT INTO comments (todo_id, user_id, comment) VALUES (2, 1, 'This is a comment on item 2');

INSERT INTO reminders (todo_id, reminder_date) VALUES (1, '2023-12-01T09:00:00');
INSERT INTO reminders (todo_id, reminder_date) VALUES (2, '2023-12-02T10:00:00');

INSERT INTO user_roles (role_name) VALUES ('Administrator');
INSERT INTO user_roles (role_name) VALUES ('User');

INSERT INTO user_roles_assignments (user_id, role_id) VALUES (1, 1);
INSERT INTO user_roles_assignments (user_id, role_id) VALUES (2, 2);