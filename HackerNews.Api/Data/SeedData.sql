-- Enable foreign keys
PRAGMA foreign_keys = ON;

-- Create Users table
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT UNIQUE NOT NULL,
    Karma INTEGER DEFAULT 0,
    About TEXT,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP
);

-- Create Items table
CREATE TABLE Items (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- Unique ID for each item
    Deleted INTEGER NULL,  -- 0 or 1 for boolean values
    Type TEXT NOT NULL CHECK (Type IN ('job', 'story', 'comment', 'poll', 'pollopt')),  -- Enum-like constraint
    ByUsername TEXT NULL,  -- Author's username
    Text TEXT NULL,  -- HTML content of the item
    Dead INTEGER NULL,  -- 0 or 1 for boolean values
    Parent INTEGER NULL,  -- Parent comment or story ID
    Poll INTEGER NULL,  -- Associated poll ID
    Url TEXT NULL,  -- URL for stories/jobs
    Score INTEGER NULL,  -- Score or votes for pollopt
    Title TEXT NULL,  -- Title for story, poll, or job
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (Parent) REFERENCES Items(Id),
    FOREIGN KEY (Poll) REFERENCES Items(Id)
);

-- Insert Users
INSERT INTO Users (Username, Karma, About, CreatedAt) VALUES
('alice', 150, 'Tech enthusiast.', '2018-06-15 08:23:45'),
('bob', 200, 'Software engineer.', '2019-11-27 16:45:30'),
('charlie', 300, 'Open-source contributor.', '2020-04-10 12:10:05'),
('dave', 250, 'AI researcher.', '2017-09-03 19:30:20'),
('eve', 180, 'Cybersecurity expert.', '2021-02-18 05:55:12');


-- Insert Stories
INSERT INTO Items (Id, Type, ByUsername, Title, Url, Score, CreatedAt) VALUES
(1, 'story', 'alice', 'New AI Breakthrough', 'https://example.com/ai', 120, '2019-07-12 10:30:00'),
(2, 'story', 'bob', 'Rust vs Go: Which is better?', 'https://example.com/rust-go', 95, '2020-01-05 14:45:00'),
(3, 'story', 'charlie', 'The Future of Web Development', NULL, 110, '2021-03-22 09:15:00'),
(4, 'story', 'dave', 'Quantum Computing Advances', 'https://example.com/quantum', 85, '2018-11-30 18:20:00'),
(5, 'story', 'eve', 'Cybersecurity in 2025: What to Expect', 'https://example.com/cybersecurity', 130, '2019-06-10 12:00:00'),
(6, 'story', 'frank', 'Is NoSQL the Future of Databases?', NULL, 90, '2021-09-15 16:10:00'),
(7, 'story', 'grace', 'Functional Programming: Why It Matters', 'https://example.com/functional', 105, '2020-08-22 08:40:00'),
(8, 'story', 'hank', 'WebAssembly: A Game Changer for the Web?', 'https://example.com/wasm', 112, '2018-12-05 20:55:00'),
(9, 'story', 'ivy', '5G and the Internet of Things', NULL, 140, '2022-04-18 07:30:00'),
(10, 'story', 'jack', 'How Blockchain is Changing Finance', 'https://example.com/blockchain', 125, '2019-02-14 13:20:00'),
(11, 'story', 'alice', 'Kubernetes vs Docker Swarm: Which One?', 'https://example.com/k8s-docker', 98, '2020-11-02 11:15:00'),
(12, 'story', 'bob', 'The Impact of AI in Medicine', 'https://example.com/ai-medicine', 111, '2018-10-29 17:45:00'),
(13, 'story', 'charlie', 'Best Practices for Microservices Architecture', 'https://example.com/microservices', 100, '2021-07-03 14:00:00'),
(14, 'story', 'dave', 'Will Python Rule Forever?', NULL, 132, '2019-12-18 15:30:00'),
(15, 'story', 'eve', 'Advancements in Neural Networks', 'https://example.com/neural-nets', 145, '2023-01-01 09:55:00'),
(16, 'story', 'frank', 'A Guide to Ethical Hacking', 'https://example.com/hacking', 102, '2018-08-10 22:10:00'),
(17, 'story', 'grace', 'Augmented Reality in Gaming', NULL, 120, '2020-05-28 07:45:00'),
(18, 'story', 'hank', 'Why TypeScript is Taking Over JavaScript', 'https://example.com/typescript', 88, '2021-12-07 10:05:00'),
(19, 'story', 'ivy', 'Is the Metaverse Overhyped?', 'https://example.com/metaverse', 115, '2022-06-25 19:30:00'),
(20, 'story', 'jack', 'The Downfall of Moore’s Law', 'https://example.com/moore-law', 97, '2019-04-03 23:50:00'),
(21, 'story', 'alice', 'The Death of Cookies in Advertising', NULL, 109, '2021-01-14 16:40:00'),
(22, 'story', 'bob', 'Deep Learning vs Traditional Machine Learning', NULL, 121, '2018-07-29 12:25:00'),
(23, 'story', 'charlie', 'Will We Ever Have Artificial General Intelligence?', NULL, 135, '2023-02-11 14:10:00'),
(24, 'story', 'dave', 'Top Programming Languages in 2025', 'https://example.com/lang-2025', 110, '2020-09-09 18:30:00'),
(25, 'story', 'eve', 'Elon Musk’s Latest Tech Vision', 'https://example.com/elon-musk', 150, '2019-03-17 10:20:00'),
(26, 'story', 'frank', 'How Fast is Starlink Internet?', 'https://example.com/starlink', 99, '2021-08-12 09:15:00'),
(27, 'story', 'grace', 'The Reality of Self-Driving Cars', 'https://example.com/self-driving', 118, '2022-05-21 22:00:00'),
(28, 'story', 'hank', 'How Quantum Cryptography Works', 'https://example.com/q-crypto', 107, '2018-06-15 13:45:00'),
(29, 'story', 'ivy', 'The Future of VR Workspaces', 'https://example.com/vr-work', 133, '2020-04-07 08:20:00'),
(30, 'story', 'jack', 'Why Rust is Gaining Popularity', 'https://example.com/rust', 101, '2023-03-10 15:35:00');

-- Insert Comments
INSERT INTO Items (Id, Type, ByUsername, Text, Parent, CreatedAt) VALUES
(31, 'comment', 'hank', 'This is amazing!', 1, '2019-08-14 10:45:00'),
(32, 'comment', 'ivy', 'AI is moving fast!', 1, '2020-02-20 15:30:00'),
(33, 'comment', 'alice', 'I love this topic!', 2, '2021-04-11 09:05:00'),
(34, 'comment', 'bob', 'Rust is awesome!', 2, '2018-12-02 20:15:00'),
(35, 'comment', 'charlie', 'Very insightful read.', 3, '2019-06-18 13:25:00'),
(36, 'comment', 'dave', 'The web is evolving so fast!', 3, '2021-09-09 08:50:00'),
(37, 'comment', 'eve', 'Quantum computing is the future!', 4, '2018-11-23 14:30:00'),
(38, 'comment', 'frank', 'Great explanation!', 4, '2020-07-15 17:45:00'),
(39, 'comment', 'grace', 'Cybersecurity is so important.', 5, '2019-05-03 12:15:00'),
(40, 'comment', 'hank', 'Good points raised here.', 5, '2022-03-14 10:50:00'),
(41, 'comment', 'ivy', 'NoSQL is indeed interesting.', 6, '2021-06-28 09:40:00'),
(42, 'comment', 'jack', 'Relational databases are still strong though.', 6, '2018-07-19 16:55:00'),
(43, 'comment', 'alice', 'Functional programming has its perks!', 7, '2020-10-05 11:30:00'),
(44, 'comment', 'bob', 'This is why I love Haskell.', 7, '2021-12-13 18:20:00'),
(45, 'comment', 'charlie', 'WebAssembly is super exciting!', 8, '2019-01-26 07:45:00'),
(46, 'comment', 'dave', 'This will change frontend development.', 8, '2022-07-08 14:10:00'),
(47, 'comment', 'eve', '5G will revolutionize IoT.', 9, '2020-03-18 09:05:00'),
(48, 'comment', 'frank', 'The speed improvements are amazing.', 9, '2021-08-02 12:40:00'),
(49, 'comment', 'grace', 'Blockchain is truly transformative.', 10, '2019-09-21 15:50:00'),
(50, 'comment', 'hank', 'Smart contracts are the future.', 10, '2022-05-30 10:15:00'),
(51, 'comment', 'ivy', 'Kubernetes makes deployment easier.', 11, '2018-10-10 17:25:00'),
(52, 'comment', 'jack', 'Docker Swarm still has use cases.', 11, '2020-06-05 11:50:00'),
(53, 'comment', 'alice', 'AI in medicine is a game-changer.', 12, '2019-02-14 09:35:00'),
(54, 'comment', 'bob', 'Hope it leads to better diagnoses.', 12, '2021-11-17 14:45:00'),
(55, 'comment', 'charlie', 'Microservices are so complex!', 13, '2018-09-01 08:30:00'),
(56, 'comment', 'dave', 'But they improve scalability.', 13, '2020-12-22 13:55:00'),
(57, 'comment', 'eve', 'Python will be around for a long time.', 14, '2019-04-27 16:40:00'),
(58, 'comment', 'frank', 'Its ecosystem is just too strong.', 14, '2022-02-09 10:05:00'),
(59, 'comment', 'grace', 'Neural networks are fascinating!', 15, '2018-06-20 07:15:00'),
(60, 'comment', 'hank', 'Their applications are endless.', 15, '2023-01-31 12:50:00');