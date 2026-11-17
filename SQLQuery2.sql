UPDATE Users SET QuotaBytes = 50000 WHERE EmailAddress = 'ian@postbox.com';

SELECT EmailAddress, QuotaBytes FROM Users WHERE EmailAddress = 'ian@postbox.com';

UPDATE Users SET QuotaBytes = NULL WHERE EmailAddress = 'ian@postbox.com';
UPDATE Users SET QuotaBytes = NULL WHERE EmailAddress = 'apako@postbox.com';


UPDATE Users SET QuotaBytes = 50000 WHERE EmailAddress = 'ian@postbox.com';