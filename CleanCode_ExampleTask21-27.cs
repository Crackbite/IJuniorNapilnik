private static AccessResult ProvideAccessToBulletin(string passport)
{
    var connectionString =
        $"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\db.sqlite";
    var connection = new SQLiteConnection(connectionString);

    try
    {
        connection.Open();

        var commandText = $"select * from passports where num='{ComputeSha256Hash(passport)}' limit 1;";
        var dataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));

        var dataTable = new DataTable();
        dataAdapter.Fill(dataTable);

        AccessResult accessResult;
        string messageText;

        if (dataTable.Rows.Count > 0)
        {
            var accessGranted = Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]);

            messageText = $"По паспорту «{passport}» доступ к бюллетеню на "
                          + "дистанционном электронном голосовании "
                          + (accessGranted ? "ПРЕДОСТАВЛЕН" : "НЕ ПРЕДОСТАВЛЯЛСЯ");

            accessResult = new AccessResult(accessGranted, new AccessMessage(messageText));
        }
        else
        {
            messageText = $"Паспорт «{passport}» в списке участников дистанционного голосования НЕ НАЙДЕН";
            accessResult = new AccessResult(false, new AccessMessage(messageText));
        }

        return accessResult;
    }
    catch (SQLiteException ex)
    {
        string messageText = ex.ErrorCode != 1
                                 ? ex.Message
                                 : "Файл db.sqlite не найден. Положите файл в папку вместе с exe.";

        return new AccessResult(false, new AccessMessage(messageText, true));
    }
    finally
    {
        connection.Close();
    }
}

private void checkButton_Click(object sender, EventArgs e)
{
    string passport = passportTextbox.Text.Trim();

    if (string.IsNullOrWhiteSpace(passport))
    {
        MessageBox.Show("Введите серию и номер паспорта");
        return;
    }

    passport = passport.Replace(" ", string.Empty);

    if (passport.Length < 10)
    {
        textResult.Text = "Неверный формат серии или номера паспорта";
        return;
    }

    AccessResult accessResult = ProvideAccessToBulletin(passport);

    if (accessResult.Success == false && accessResult.Message.IsMessageBox)
    {
        MessageBox.Show(accessResult.Message.Text);
    }
    else
    {
        textResult.Text = accessResult.Message.Text;
    }
}

internal class AccessResult
{
    public AccessResult(bool success, AccessMessage message)
    {
        Success = success;
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public AccessMessage Message { get; }

    public bool Success { get; }
}

internal class AccessMessage
{
    public AccessMessage(string text, bool isMessageBox = false)
    {
        Text = text;
        IsMessageBox = isMessageBox;
    }

    public bool IsMessageBox { get; }

    public string Text { get; }
}