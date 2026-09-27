# Inventory Checker

A Windows Forms app for checking a small CSV inventory with a barcode/QR scanner that types into the keyboard.

## Build and run

Requires Windows and the .NET 10 SDK to build. The built app requires the .NET 10 Desktop Runtime. Microsoft supports [.NET 10 LTS through November 2028](https://dotnet.microsoft.com/en-us/platform/support/policy).

```powershell
dotnet build InventoryChecker.csproj -c Release
dotnet run --project InventoryChecker.csproj
```

The release executable is `bin/Release/net10.0-windows/InventoryChecker.exe`. Keep the other files in that folder with it when copying the app. Microsoft Office and third-party NuGet packages are no longer required.

Run the regression suite:

```powershell
dotnet run --project tests/InventoryChecker.RegressionTests -c Release
```

## Checking inventory

1. Click **Load Data** and choose a CSV file with a header row and at least one data row.
2. Choose a checking field that uniquely identifies each row. A field named `Code` is selected automatically when present.
3. Click **Scan Code**. Configure the scanner to send **Enter** after each code, or type a code and press Enter. The dialog stays open and displays the result immediately. Press **Escape** or **Close** to finish scanning.
4. Use **Search** to find literal text in a selected field. Single-character searches work; clear the text and search again to show all rows. Starting a scan session also clears the search.
5. Use **Export Results** to save the entire inventory, including rows hidden by a search, as `.xlsx` or `.csv`. The status bar shows progress; an asterisk in the title marks unsaved checks. Closing or loading another file offers to export unsaved results.

Each row represents one item to check, not a quantity counter. A repeated scan reports **Already checked**. Blank and unknown codes leave inventory unchanged. If several rows match a scan, none is changed: choose a unique field or fix the source CSV. Case-insensitive matching is the default and is independent of Windows language settings; enable **Case-sensitive** if codes differ by case.

## File format

```csv
Code,Description,Location
000123,Small widget,Shelf A
000124,"Widget, large",Shelf B
```

- Input is comma-delimited UTF-8 (BOM optional); BOM-marked Unicode files are also supported. Legacy ANSI encodings and semicolon-separated files should first be converted to UTF-8 CSV.
- Headers must be nonblank and unique ignoring case. Every record must have the same number of fields as the header. Quoted commas, quotes, line breaks, Unicode, leading zeros, and surrounding whitespace are preserved. Blank physical lines are skipped.
- `Checked` is reserved for saved progress. It accepts `true`, `false`, `1`, `0`, or an empty value (unchecked), ignoring case. If absent, all rows start unchecked. Invalid files show an error and leave the current inventory intact.
- Codes match the full field value, including surrounding spaces. The app does not convert identifiers to numbers or trim them.
- **CSV** exports can be loaded again to resume checking. They preserve text exactly. When opening CSV directly in spreadsheet software, that software can interpret leading zeros and formula-like text; choose **Excel workbook** when exporting for spreadsheet use.
- **Excel workbook** exports store inventory values as text and check states as booleans, preserving long codes and leading zeros and preventing source text from becoming formulas. `.xlsx` import is not supported. Excel cell and worksheet limits are validated before an export replaces an existing file.
- Export writes to a temporary file in the destination folder before replacing the destination. Serialization failures preserve an existing destination. The original input file is changed only if you explicitly choose it as the export destination and confirm overwrite.

## Maintenance changes

The old Excel COM export failed under `dotnet build`, required Excel, changed the clipboard, and exported only visible grid rows. Export now creates files directly. The obsolete Accord CSV dependency, old .NET Framework configuration, and asynchronous success popup have been removed. Scanner input stays focused between submissions, search expressions escape literal input, imports validate before replacing the active table, and matching rejects ambiguous codes. The main window is resizable and the grid remains read-only to protect source values.

The regression runner covers file round trips and validation, damaged encodings, failed-export recovery, Excel text/boolean output, literal searches, scanner matching and duplicates, and WinForms interaction checks. A physical scanner still needs a final check on the target machine, especially its Enter suffix and keyboard layout.
