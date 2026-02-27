-- Initialize database for training
CREATE TABLE IF NOT EXISTS training_logs (
    id SERIAL PRIMARY KEY,
    message TEXT NOT NULL,
    log_level VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS api_requests (
    id SERIAL PRIMARY KEY,
    endpoint VARCHAR(255),
    method VARCHAR(10),
    status_code INTEGER,
    response_time_ms INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for performance
CREATE INDEX IF NOT EXISTS idx_training_logs_created ON training_logs(created_at);
CREATE INDEX IF NOT EXISTS idx_api_requests_created ON api_requests(created_at);

-- Sample data
INSERT INTO training_logs (message, log_level) VALUES
    ('Docker Compose training started', 'INFO'),
    ('Database initialized', 'INFO'),
    ('Services ready', 'INFO');
