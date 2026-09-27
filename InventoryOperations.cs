using System.Data;
using System.Text;

namespace InventoryChecker;

internal enum ScanResult
{
    Checked,
    AlreadyChecked,
    NotFound,
    Ambiguous,
    EmptyCode
}

internal sealed record ScanOutcome(ScanResult Result, DataRow? Row, int MatchCount);

internal static class InventoryOperations
{
    public static string BuildSearchFilter(string columnName, string keyword)
    {
        if (keyword.Length == 0)
            return string.Empty;

        // DataView uses an expression language: escape both identifiers and LIKE literals.
        string identifier = columnName.Replace("\\", "\\\\").Replace("]", "\\]");
        var literal = new StringBuilder();
        foreach (char character in keyword)
        {
            literal.Append(character switch
            {
                '\'' => "''",
                '[' => "[[]",
                ']' => "[]]",
                '*' => "[*]",
                '%' => "[%]",
                _ => character.ToString()
            });
        }

        return $"[{identifier}] LIKE '%{literal}%'";
    }

    public static ScanOutcome CheckItem(DataTable table, string fieldName, string code, bool caseSensitive)
    {
        if (string.IsNullOrWhiteSpace(code))
            return new(ScanResult.EmptyCode, null, 0);

        StringComparison comparison = caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
        DataRow? match = null;
        int matchCount = 0;
        foreach (DataRow row in table.Rows)
        {
            if (row.IsNull(fieldName) || !string.Equals(row[fieldName].ToString(), code, comparison))
                continue;

            match = row;
            matchCount++;
        }

        if (matchCount == 0 || match == null)
            return new(ScanResult.NotFound, null, 0);
        // The chosen field must identify one row; never silently check an arbitrary duplicate.
        if (matchCount > 1)
            return new(ScanResult.Ambiguous, null, matchCount);
        if (match.Field<bool>(InventoryFile.CheckedColumnName))
            return new(ScanResult.AlreadyChecked, match, 1);

        match[InventoryFile.CheckedColumnName] = true;
        return new(ScanResult.Checked, match, 1);
    }
}
