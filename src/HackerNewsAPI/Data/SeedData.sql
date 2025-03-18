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
    Id INTEGER PRIMARY KEY,  -- Unique ID for each item
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

-- Insert Users with random CreatedAt
INSERT INTO Users (Username, Karma, About, CreatedAt) VALUES
('alice', 150, 'Tech enthusiast.', datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
('bob', 200, 'Software engineer.', datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
('charlie', 300, 'Open-source contributor.', datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
('dave', 250, 'AI researcher.', datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
('eve', 180, 'Cybersecurity expert.', datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days'));

-- Insert Stories
INSERT INTO Items (Id, Type, ByUsername, Title, Url, Score, CreatedAt) VALUES
(1, 'story', 'alice', 'New AI Breakthrough', 'https://example.com/ai', 120, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(2, 'story', 'bob', 'Rust vs Go: Which is better?', 'https://example.com/rust-go', 95, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(3, 'story', 'charlie', 'The Future of Web Development', NULL, 110, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(4, 'story', 'dave', 'Quantum Computing Advances', 'https://example.com/quantum', 85, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(5, 'story', 'eve', 'Cybersecurity in 2025: What to Expect', 'https://example.com/cybersecurity', 130, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(6, 'story', 'frank', 'Is NoSQL the Future of Databases?', NULL, 90, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(7, 'story', 'grace', 'Functional Programming: Why It Matters', 'https://example.com/functional', 105, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(8, 'story', 'hank', 'WebAssembly: A Game Changer for the Web?', 'https://example.com/wasm', 112, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(9, 'story', 'ivy', '5G and the Internet of Things', NULL, 140, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(10, 'story', 'jack', 'How Blockchain is Changing Finance', 'https://example.com/blockchain', 125, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(11, 'story', 'alice', 'Kubernetes vs Docker Swarm: Which One?', 'https://example.com/k8s-docker', 98, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(12, 'story', 'bob', 'The Impact of AI in Medicine', 'https://example.com/ai-medicine', 111, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(13, 'story', 'charlie', 'Best Practices for Microservices Architecture', 'https://example.com/microservices', 100, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(14, 'story', 'dave', 'Will Python Rule Forever?', NULL, 132, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(15, 'story', 'eve', 'Advancements in Neural Networks', 'https://example.com/neural-nets', 145, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(16, 'story', 'frank', 'A Guide to Ethical Hacking', 'https://example.com/hacking', 102, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(17, 'story', 'grace', 'Augmented Reality in Gaming', NULL, 120, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(18, 'story', 'hank', 'Why TypeScript is Taking Over JavaScript', 'https://example.com/typescript', 88, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(19, 'story', 'ivy', 'Is the Metaverse Overhyped?', 'https://example.com/metaverse', 115, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(20, 'story', 'jack', 'The Downfall of Moore’s Law', 'https://example.com/moore-law', 97, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(21, 'story', 'alice', 'The Death of Cookies in Advertising', NULL, 109, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(22, 'story', 'bob', 'Deep Learning vs Traditional Machine Learning', NULL, 121, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(23, 'story', 'charlie', 'Will We Ever Have Artificial General Intelligence?', NULL, 135, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(24, 'story', 'dave', 'Top Programming Languages in 2025', 'https://example.com/lang-2025', 110, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(25, 'story', 'eve', 'Elon Musk’s Latest Tech Vision', 'https://example.com/elon-musk', 150, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(26, 'story', 'frank', 'How Fast is Starlink Internet?', 'https://example.com/starlink', 99, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(27, 'story', 'grace', 'The Reality of Self-Driving Cars', 'https://example.com/self-driving', 118, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(28, 'story', 'hank', 'How Quantum Cryptography Works', 'https://example.com/q-crypto', 107, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(29, 'story', 'ivy', 'The Future of VR Workspaces', 'https://example.com/vr-work', 133, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(30, 'story', 'jack', 'Why Rust is Gaining Popularity', 'https://example.com/rust', 101, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days'));



-- Insert Comments
INSERT INTO Items (Id, Type, ByUsername, Text, Parent, CreatedAt) VALUES
(31, 'comment', 'hank', 'This is amazing!', 1, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(32, 'comment', 'ivy', 'AI is moving fast!', 1, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(33, 'comment', 'alice', 'I love this topic!', 2, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(34, 'comment', 'bob', 'Rust is awesome!', 2, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(35, 'comment', 'charlie', 'Very insightful read.', 3, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(36, 'comment', 'dave', 'The web is evolving so fast!', 3, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(37, 'comment', 'eve', 'Quantum computing is the future!', 4, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(38, 'comment', 'frank', 'Great explanation!', 4, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(39, 'comment', 'grace', 'Cybersecurity is so important.', 5, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(40, 'comment', 'hank', 'Good points raised here.', 5, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(41, 'comment', 'ivy', 'NoSQL is indeed interesting.', 6, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(42, 'comment', 'jack', 'Relational databases are still strong though.', 6, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(43, 'comment', 'alice', 'Functional programming has its perks!', 7, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(44, 'comment', 'bob', 'This is why I love Haskell.', 7, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(45, 'comment', 'charlie', 'WebAssembly is super exciting!', 8, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(46, 'comment', 'dave', 'This will change frontend development.', 8, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(47, 'comment', 'eve', '5G will revolutionize IoT.', 9, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(48, 'comment', 'frank', 'The speed improvements are amazing.', 9, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(49, 'comment', 'grace', 'Blockchain is truly transformative.', 10, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(50, 'comment', 'hank', 'Smart contracts are the future.', 10, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(51, 'comment', 'ivy', 'Kubernetes makes deployment easier.', 11, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(52, 'comment', 'jack', 'Docker Swarm still has use cases.', 11, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(53, 'comment', 'alice', 'AI in medicine is a game-changer.', 12, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(54, 'comment', 'bob', 'Hope it leads to better diagnoses.', 12, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(55, 'comment', 'charlie', 'Microservices are so complex!', 13, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(56, 'comment', 'dave', 'But they improve scalability.', 13, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(57, 'comment', 'eve', 'Python will be around for a long time.', 14, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(58, 'comment', 'frank', 'Its ecosystem is just too strong.', 14, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(59, 'comment', 'grace', 'Neural networks are fascinating!', 15, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days')),
(60, 'comment', 'hank', 'Their applications are endless.', 15, datetime('now', '-5 years', '+' || abs(random() % 1825) || ' days'));
