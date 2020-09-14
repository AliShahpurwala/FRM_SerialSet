using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace FRM_SerialSet
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SerialComm globalSerialCode = new SerialComm();



        private void OK_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("The Serial Port has not been connected yet.");
            }
            else
            {
                if (globalSerialCode.result() == "@00RD*\n")
                {
                    MessageBox.Show("You have not created the query string as yet.");
                }
                else
                {
                    serialPort1.Write(globalSerialCode.result());

                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter the start read location in the textbox");
        }

        #region Connection Details Rendering
        private void connectButton_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                this.connectButton.Text = "Connect";
                this.connectionProgressBar.Value = 0;
                
            }
            else
            {
                Form connectionDetails = new Form();
                connectionDetails.Height = 300;
                connectionDetails.Width = 300;
                connectionDetails.Text = "Connection Details";

                Label comportInputLabel = new Label() { Top = 20, Left = 20, Text = "Comport" };
                ComboBox comportInput = new ComboBox() { Top = 20, Left = 120 };
                comportInput.DropDownStyle = ComboBoxStyle.DropDownList;

                // Adding list items in the comportInput Drop Down

                String[] availableComports = SerialPort.GetPortNames();
                comportInput.Items.AddRange(availableComports);

                Label baudInputLabel = new Label() { Top = 50, Left = 20, Text = "Baud" };
                TextBox baudInput = new TextBox() { Top = 50, Left = 120 };

                Label parityInputLabel = new Label() { Top = 80, Left = 20, Text = "Parity" };
                ComboBox parityInput = new ComboBox() { Top = 80, Left = 120 };
                parityInput.DropDownStyle = ComboBoxStyle.DropDownList;


                // Adding list items in the parityInput Drop Down
                String[] validParity = { "Odd", "Even", "None"};
                parityInput.Items.AddRange(validParity);


                Label stopBitInputLabel = new Label() { Top = 110, Left = 20, Text = "Stop Bit" };
                ComboBox stopBitInput = new ComboBox() { Top = 110, Left = 120 };
                stopBitInput.DropDownStyle = ComboBoxStyle.DropDownList;

                // Adding list items in the stopBitInput Drop Down
                String[] validStopBits = { "None", "One", "Two" };
                stopBitInput.Items.AddRange(validStopBits);

                //TextBox stopBitInput = new TextBox() { Top = 110, Left = 120 };

                Button promptOK = new Button() { Text = "OK", Top = 200, Left = 120 };
                Button promptCancel = new Button() { Text = "Cancel", Top = 200, Left = 200 };

                string comport, parity, baud, stopBit;
                promptOK.Click += (sender1, e1) =>
                {
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
                        Parity par = Parity.None;
                        if (parity.Trim() == "Odd")
                        {
                            par = Parity.Odd;
                        }
                        else if (parity.Trim() == "Even")
                        {
                            par = Parity.Even;
                        }
                        else if (parity.Trim() == "None")
                        {
                            par = Parity.None;
                        }
                        
                        StopBits stopB = StopBits.None;
                        if (stopBit.Trim() == "One")
                        {
                            stopB = StopBits.One;
                        }
                        else if (stopBit.Trim() == "Two")
                        {
                            stopB = StopBits.Two;
                        }
                        else if (stopBit.Trim() == "None")
                        {
                            stopB = StopBits.None;
                        }

                        serialPort1.BaudRate = Int32.Parse(baud);
                        serialPort1.Parity = par;
                        serialPort1.StopBits = stopB;

                        updateCommDetailMainForm();

                        connectionDetails.Close();
                    }

                };
                promptCancel.Click += (sender1, e1) =>
                {
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
        }

        private int verifySerialCommSettingInput(string comport, string baud, string parity, string stopBit)
        {
            
            return 100;
        }
        #endregion

        #region Error Message Handling
        private string humanReadableErrorMessage(int errorCode)
        {
            if (errorCode == 150)
            {
                return "Error 150: Parity can only be either odd or even.";
            }
            else if (errorCode == 200)
            {
                return "Error 200: Comport must be either 1, 2 or 3.";
            }
            else if (errorCode == 250)
            {
                return "Error 250: Stop Bit must be either 1, 2 or 3.";
            }
            return "No Error Found!";
        }
        #endregion




        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void generateCommCode_Click(object sender, EventArgs e)
        {
            String startReadLocation = this.NameInput.Text;
            String readRangeInput = this.readDataRange.Text;
            SerialComm mySerialCode = new SerialComm(startReadLocation, readRangeInput);
            globalSerialCode = mySerialCode;
            this.generatedCommCodeLabel.Text = mySerialCode.result();
            this.generatedCommCodeLabel.BorderStyle = BorderStyle.FixedSingle;

        }

        private void updateCommDetailMainForm()
        {
            this.comportLabel.Text = serialPort1.PortName;
            this.baudLabel.Text = serialPort1.BaudRate.ToString();
            this.parityLabel.Text = serialPort1.Parity.ToString();
            this.stopBitLabel.Text = serialPort1.StopBits.ToString();

            try
            {
                serialPort1.Open();
                this.connectButton.Text = "Disconnect";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.connectionProgressBar.Value = 100;
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

        public SerialComm()
        {
            this.startPosition = "";
            this.positionCount = "";
            this.FCS = "";
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