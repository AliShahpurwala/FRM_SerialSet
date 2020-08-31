using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FRM_SerialSet
{
    
    public partial class Form1 : Form
    {
        connectionStatus ConnectionStatus = new connectionStatus();
        public Form1()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            String givenName = this.NameInput.Text;
            MessageBox.Show(givenName);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the start read location in the textbox");
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Form connectionDetails = new Form();
            connectionDetails.Height = 300;
            connectionDetails.Width = 300;
            connectionDetails.Text = "Connection Details";
            
            Label comportInputLabel = new Label() { Top = 20, Left = 20, Text = "Comport" };
            TextBox comportInput = new TextBox() { Top = 20, Left = 120};

            Label baudInputLabel = new Label() { Top = 50, Left = 20, Text = "Baud" };
            TextBox baudInput = new TextBox() { Top = 50, Left = 120 };

            Label parityInputLabel = new Label() { Top = 80, Left = 20, Text = "Parity" };
            TextBox parityInput = new TextBox() { Top = 80, Left = 120 };

            Label stopBitInputLabel = new Label() { Top = 110, Left = 20, Text = "Stop Bit" };
            TextBox stopBitInput = new TextBox() { Top = 110, Left = 120 };

            Button promptOK = new Button() { Text = "OK", Top = 200, Left = 120 };
            Button promptCancel = new Button() { Text = "Cancel", Top = 200, Left = 200 };

            string comport, parity, baud, stopBit;
            promptOK.Click += (sender1, e1) => {
                comport = comportInput.Text;
                parity = parityInput.Text;
                baud = baudInput.Text;
                stopBit = stopBitInput.Text;

                int errorStatus = verifySerialCommSettingInput(comport, baud, parity, stopBit);

                if (errorStatus != 100)
                {
                    MessageBox.Show(humanReadableErrorMessage(errorStatus));
                }
                else
                {
                    connectionDetails.Close();
                }
                
            };
            promptCancel.Click += (sender1, e1) => {
                connectionDetails.Close();
            };
            connectionDetails.Controls.Add(comportInputLabel);
            connectionDetails.Controls.Add(comportInput);
            connectionDetails.Controls.Add(baudInput);
            connectionDetails.Controls.Add(baudInputLabel);
            connectionDetails.Controls.Add(parityInput);
            connectionDetails.Controls.Add(parityInputLabel);
            connectionDetails.Controls.Add(stopBitInputLabel);
            connectionDetails.Controls.Add(stopBitInput);
            connectionDetails.Controls.Add(promptOK);
            connectionDetails.Controls.Add(promptCancel);
            connectionDetails.ShowDialog();

            
            
            
        }

        private int verifySerialCommSettingInput(string comport, string baud, string parity, string stopBit)
        {
            parity = parity.Trim();
            if (parity.ToLower() != "odd" && parity.ToLower() != "even")
            {
                return 150;
            }
            return 100;
        }

        private string humanReadableErrorMessage(int errorCode)
        {
            if (errorCode == 150)
            {
                return "Error 150: Parity can only be either odd or even.";
            }
            return "TODO: Implement Function";
        }
    }

    public class connectionStatus
    {
        bool status;

        public connectionStatus()
        {
            this.status = false;
        }

        private void changeStatus()
        {
            if (this.status == false)
            {
                this.status = true;
            }
            else
            {
                this.status = false;
            }
        }
    }

    #region Serial Communication Class - User Defined
    class SerialComm
    {
        private char startKey = '@';
        private string nodeNum = "00";
        private string headerCode = "RD";
        private string startPosition;
        private string positionCount;
        private string FCS;
        private string endKey = "*\n";
        private bool fuse = false;
        public bool Fuse
        {
            get { return fuse; }
        }

        public SerialComm(string startPosition, string positionCount)
        {
            if (validatePosition(fillPosition(startPosition)))
            {
                this.startPosition = fillPosition(startPosition);
            }
            else
            {
                this.fuse = true;
            }
            if (validatePosition(fillPosition(positionCount)))
            {
                this.positionCount = fillPosition(positionCount);
            }
            else
            {
                this.fuse = true;
            }
            if (!this.fuse)
            {
                if (outOfBounds(Int32.Parse(this.startPosition), Int32.Parse(this.positionCount)))
                {
                    this.fuse = true;
                }
            }
            if (!this.fuse)
            {
                this.FCS = fcsCalculate();
            }


        }

        private string fcsCalculate()
        {
            string eval = this.startKey + this.nodeNum + this.headerCode + this.startPosition + this.positionCount;
            char[] evalArray = eval.ToCharArray();
            bool[] result = new bool[8];
            bool[] compare = new bool[8];
            foreach (char ch in evalArray)
            {
                compare = charToBin(ch);
                for (int r = 0; r < 8; r++)
                {
                    result[r] = result[r] ^ compare[r];
                }
            }
            return binToChar(result);
        }
        private string binToChar(bool[] input)
        {
            double result = 0;
            double result2 = 0;
            bool[] firstSet = new bool[4];
            bool[] secondSet = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                firstSet[i] = input[i];
            }
            for (int y = 0; y < 4; y++)
            {
                secondSet[y] = input[y + 4];
            }
            for (int t = 0; t < 4; t++)
            {
                if (firstSet[t])
                {
                    result += Math.Pow(2, 3 - t);
                }
                if (secondSet[t])
                {
                    result2 += Math.Pow(2, 3 - t);
                }
            }
            string final = result.ToString() + result2.ToString();
            return final;
        }
        private bool[] charToBin(char c)
        {
            bool[] result = new bool[8];
            int charCode = (int)c;
            string binaryValue = fillBinary(Convert.ToString(charCode, 2));
            char[] binaryValueArray = binaryValue.ToCharArray();
            for (int y = 0; y < binaryValueArray.Length; y++)
            {
                if (binaryValueArray[y] == '1')
                {
                    result[y] = true;
                }
                else
                {
                    result[y] = false;
                }
            }
            return result;
        }
        private string fillBinary(string s)
        {
            int numEmpty = 8 - s.Length;
            for (int i = 0; i < numEmpty; i++)
            {
                s = "0" + s;
            }
            return s;
        }
        public string result()
        {
            if (this.fuse)
            {
                return "Object was not created succesfully";
            }
            else
            {
                return this.startKey + this.nodeNum + this.headerCode + this.startPosition + this.positionCount + this.FCS + this.endKey;
            }
        }
        private bool outOfBounds(int startPos, int endPos)
        {
            if (startPos + endPos > 9999)
            {
                return true;
            }
            return false;
        }
        private bool validatePosition(string position)
        {
            if (position.Length > 4)
            {
                return false;
            }
            try
            {
                Int32.Parse(position);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private string fillPosition(string position)
        {
            if (position.Length == 1)
            {
                return "000" + position;
            }
            else if (position.Length == 2)
            {
                return "00" + position;
            }
            else if (position.Length == 3)
            {
                return "0" + position;
            }
            else
            {
                return position;
            }

        }
    }
    #endregion
}
