using System.IO;
using System.Text;

namespace Snowflake.Plugin.FileSignatures.SFOSharp
{
    public sealed class SFOIndexTableEntry
    {
	    public static int indexTableEntryLength = 16;
	
	    private byte[] offsetKeyNameInKeyTable;
	    private byte dataAlignmentRequirements;
	    private byte dataTypeValue;
	    private int sizeValueData;
	    private int sizeValueDataAndPadding;
	    private int offsetDataValueInDataTable;
	
	    /**
	     * Reads one entry of the indexTable and return it's values in a SFOIndexTableEntry-object
	     * @param fIn
	     * @return SFOIndexTableEntry
	     * @throws IOException
	     */
	    public static SFOIndexTableEntry ReadEntry(Stream fIn) {
		    SFOIndexTableEntry sfoIndexTableEntry = new SFOIndexTableEntry();
		
		    byte[] tempByteArray1 = new byte[1];
		    byte[] tempByteArray2 = new byte[2];
		    byte[] tempByteArray4 = new byte[4];
		
		    // read offsetKeyNameInKeyTable
		    fIn.Read(tempByteArray2,0,2);
		    sfoIndexTableEntry.SetOffsetKeyNameInKeyTable(tempByteArray2);
		
		    // read dataAlignmentRequirements
		    fIn.Read(tempByteArray1,0,1);
		    sfoIndexTableEntry.SetDataAlignmentRequirements(tempByteArray1[0]);
		
		    // read dataTypeValue
		    fIn.Read(tempByteArray1,0,1);
		    sfoIndexTableEntry.SetDataTypeValue(tempByteArray1[0]);
		
		
		    // read sizeValueData
		    fIn.Read(tempByteArray4,0,4);
		    sfoIndexTableEntry.SetSizeValueData(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4));
		
		    // read sizeValueDataAndPadding
		    fIn.Read(tempByteArray4,0,4);
		    sfoIndexTableEntry.SetSizeValueDataAndPadding(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4));
		
		    // read offsetDataValueInDataTable
		    fIn.Read(tempByteArray4,0,4);
		    sfoIndexTableEntry.SetOffsetDataValueInDataTable(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4));
		
		    return sfoIndexTableEntry;
	    }

	    public byte[] GetOffsetKeyNameInKeyTable() {
		    return this.offsetKeyNameInKeyTable;
	    }

	    public void SetOffsetKeyNameInKeyTable(byte[] offsetKeyNameInKeyTable) {
		    this.offsetKeyNameInKeyTable = offsetKeyNameInKeyTable;
	    }

	    public byte GetDataAlignmentRequirements() {
		    return this.dataAlignmentRequirements;
	    }

	    public void SetDataAlignmentRequirements(byte dataAlignmentRequirements) {
		    this.dataAlignmentRequirements = dataAlignmentRequirements;
	    }

	    public byte GetDataTypeValue() {
		    return this.dataTypeValue;
	    }

	    public void SetDataTypeValue(byte dataTypeValue) {
		    this.dataTypeValue = dataTypeValue;
	    }

	    public int GetSizeValueData() {
		    return this.sizeValueData;
	    }

	    public void SetSizeValueData(int sizeValueData) {
		    this.sizeValueData = sizeValueData;
	    }

	    public int GetSizeValueDataAndPadding() {
		    return this.sizeValueDataAndPadding;
	    }

	    public void SetSizeValueDataAndPadding(int sizeValueDataAndPadding) {
		    this.sizeValueDataAndPadding = sizeValueDataAndPadding;
	    }

	    public int GetOffsetDataValueInDataTable() {
		    return this.offsetDataValueInDataTable;
	    }

	    public void SetOffsetDataValueInDataTable(int offsetDataValueInDataTable) {
		    this.offsetDataValueInDataTable = offsetDataValueInDataTable;
	    }
	
	    public override string ToString() {

		    StringBuilder sb = new StringBuilder();
		
		    sb.Append("== SFO IndexTable Entry ==\n")
		    .Append("offsetKeyNameInKeyTable:    ").Append(this.offsetKeyNameInKeyTable).Append("\n")
		    .Append("dataAlignmentRequirements:  ").Append(this.dataAlignmentRequirements).Append("\n")
		    .Append("dataTypeValue:              ").Append(this.dataTypeValue).Append("\n")
		    .Append("sizeValueData:              ").Append(this.sizeValueData).Append("\n")
		    .Append("sizeValueDataAndPadding:    ").Append(this.sizeValueDataAndPadding).Append("\n")
		    .Append("offsetDataValueInDataTable: ").Append(this.offsetDataValueInDataTable).Append("\n");
	
		    return sb.ToString();
	    }
    }
}
