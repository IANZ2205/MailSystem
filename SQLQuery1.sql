UPDATE Messages
SET ExpiresAt = DATEADD(MINUTE, -1, GETUTCDATE())
WHERE Subject = 'This is my secret message';