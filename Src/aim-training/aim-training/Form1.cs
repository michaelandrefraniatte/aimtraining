using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Collections.Generic;
using EO.WebBrowser;
namespace aim_training
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static int width = Screen.PrimaryScreen.Bounds.Width;
        private static int height = Screen.PrimaryScreen.Bounds.Height;
        public static Form1 form = (Form1)Application.OpenForms["Form1"];
        public static string txt, path, readText;
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.pictureBox1.Dock = DockStyle.Fill;
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            EO.WebBrowser.Runtime.DefaultEngineOptions.SetDefaultBrowserOptions(options);
            EO.WebEngine.Engine.Default.Options.AllowProprietaryMediaFormats();
            EO.WebEngine.Engine.Default.Options.SetDefaultBrowserOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Create(pictureBox1.Handle);
            this.webView1.Engine.Options.AllowProprietaryMediaFormats();
            this.webView1.SetOptions(new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            });
            this.webView1.Engine.Options.DisableGPU = false;
            this.webView1.Engine.Options.DisableSpellChecker = true;
            this.webView1.Engine.Options.CustomUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            this.webView1.KeyDown += WebView1_KeyDown;
            path = @"ppia.html";
            readText = DecryptFiles(path + ".encrypted", "tybtrybrtyertu50727885");
            webView1.LoadHtml(readText);
        }
        private void WebView1_KeyDown(object sender, EO.Base.UI.WndMsgEventArgs e)
        {
            Keys key = (Keys)e.WParam;
            OnKeyDown(key);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e.KeyData);
        }
        private void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
        }
        public static string DecryptFiles(string inputFile, string password)
        {
            using (var input = File.OpenRead(inputFile))
            {
                byte[] salt = new byte[8];
                input.Read(salt, 0, salt.Length);
                using (var decryptedStream = new MemoryStream())
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var decryptor = aes.CreateDecryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var cs = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                {
                    string contents;
                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        decryptedStream.WriteByte((byte)data);
                    decryptedStream.Position = 0;
                    using (StreamReader sr = new StreamReader(decryptedStream))
                        contents = sr.ReadToEnd();
                    decryptedStream.Flush();
                    return contents;
                }
            }
        }
        private void webView1_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            Task.Run(() => LoadPage());
        }
        private void LoadPage()
        {
            string stringinject;
            stringinject = @"
    <style>
html {
    cursor: crosshair;
}

body {
    font-family: sans-serif;
    background-color: #222222;
    color: #FFFFFF;
    cursor: crosshair;
}

.row > .col-lg-4, .col-6 {
    padding: 0;
}

#button-center {
    position: absolute;
    background-color: red;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 20px;
    height: 20px;
    cursor: crosshair;
}

#button-middle {
    position: absolute;
    background-color: white;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 40px;
    height: 40px;
    cursor: crosshair;
}

#button-border {
    position: absolute;
    background-color: red;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 60px;
    height: 60px;
    cursor: crosshair;
}

#button-outer {
    position: absolute;
    background-color: white;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 80px;
    height: 80px;
    cursor: crosshair;
}

#button-edge {
    position: absolute;
    background-color: red;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 100px;
    height: 100px;
    cursor: crosshair;
}

#shoot, .shooted {
    position: absolute;
    background-color: black;
    outline: none;
    border-radius: 100%;
    border: none;
    width: 6px;
    height: 6px;
    cursor: crosshair;
}

#start {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    border: 3px solid white;
    align-items: center;
    cursor: crosshair;
}
    </style>
".Replace("\r\n", " ");
            stringinject = @"""" + stringinject + @"""";
            stringinject = @"$(" + stringinject + @" ).appendTo('head');";
            this.webView1.EvalScript(stringinject);
            string shootmp3 = "file:///" + System.Reflection.Assembly.GetEntryAssembly().Location.Replace(@"file:\", "").Replace(Process.GetCurrentProcess().ProcessName + ".exe", "").Replace(@"\", "/").Replace(@"//", "") + "assets/shoot.mp3";
            stringinject = @"

    <div id='start'>
        <p id='go'>START</p>
    </div>

    <div id='target' style='display:none;'>
        <div id='button-edge'>
            <div id='button-outer'>
                <div id='button-border'>
                    <div id='button-middle'>
                        <div id='button-center'><div id='shoot'></div></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

<script>
var posx = 0;
var posy = 0;
var posrandx = 0;
var posrandy = 0;
var resx = 0;
var resy = 0;
var randx = 0;
var randy = 0;
var movefaster = 1000;
var go = false;
var score = 0;
var timer = 0;
var refreshIntervalId;
var shootedX = new Array;
var shootedY = new Array;

function sound(){
    new Audio('shootmp3').play();
}

$(document).click(function(){
    sound();
});

$('#button-center').click(function(e) {
    score = score + 5; 
});

$('#button-middle').click(function(e) {
    score = score + 4; 
});

$('#button-border').click(function(e) {
    score = score + 3; 
});

$('#button-outer').click(function(e) {
    score = score + 2;  
});

$('#button-edge').click(function(e) {
    score = score + 1;  
    var bounds = e.target.getBoundingClientRect();
    var shootposX  = e.pageX - this.offsetLeft;
    var shootposY = e.pageY - this.offsetTop;
    var t = document.getElementById('button-center');
    var s = document.createElement('div');
    s.setAttribute('id', 'shooted-' + score);
    var x = (shootposX + 7 - 50).toString();
    var y = (shootposY + 7 - 50).toString();
    s.className = '.shooted';
    s.setAttribute('style', `left:` + x + `px;top:` + y + `px;position:absolute;
    background-color:black;
    outline:none;
    border-radius:100%;
    border:none;
    width:6px;
    height:6px;
    cursor:crosshair;`);
    t.appendChild(s);
    $('#shoot').css({left:shootposX + 7 - 50, top:shootposY + 7 - 50});
});

$('#go').click(function() {
    var p = document.getElementById('button-center');
    removeAllChildNodes(p);
    timer = 0;
    score = 0;
    go = true;
    moveTarget();
    document.getElementById('target').style.display = 'block';
    document.getElementById('start').style.display = 'none';
    document.body.style.cursor = 'crosshair';
    refreshIntervalId = setInterval(function(){
        moveTarget();
    }, movefaster);
});

function moveTarget() {
    resx = $(document).width();
    resy = $(document).height();
    randx = Math.floor(Math.random() * (resx - 200)) + 50;
    randy = Math.floor(Math.random() * (resy - 200)) + 50;
    posrandx = randx;
    posrandy = randy;
    $('#button-center').css({left:10, top:10});
    $('#button-middle').css({left:10, top:10});
    $('#button-border').css({left:10, top:10});
    $('#button-outer').css({left:10, top:10});
    $('#button-edge').css({left:posrandx - 50, top:posrandy - 50});
    document.body.style.cursor = 'crosshair';
    timer++;
    if (timer == 31) {
        clearInterval(refreshIntervalId);
        movefaster = 900;
        refreshIntervalId = setInterval(function(){
            animateTarget();
        }, movefaster);
    }
}

function animateTarget() {
    if (timer < 60) {
        resx = $(document).width();
        resy = $(document).height();
        randx = Math.floor(Math.random() * (resx - 200)) + 50;
        randy = Math.floor(Math.random() * (resy - 200)) + 50;
        posrandx = randx;
        posrandy = randy;
        $('#button-center').animate({left:10, top:10}, movefaster);
        $('#button-middle').animate({left:10, top:10}, movefaster);
        $('#button-border').animate({left:10, top:10}, movefaster);
        $('#button-outer').animate({left:10, top:10}, movefaster);
        $('#button-edge').animate({left:posrandx - 50, top:posrandy - 50}, movefaster);
    }
    timer++;
    if (timer > 60) {
        document.getElementById('start').style.display = 'flex';
        if (score < 500) {
            $('#go').html('SCORE : ' + score);
        }
        if (score >= 500) {
            $('#go').html('SCORE : ' + score + ', NICE !');
        }
        go = false;
        clearInterval(refreshIntervalId);
        $('#button-edge').animate({left:$(document).width() / 2 - 50, top:$(document).height() / 2 + 25}, 0);
    }
    document.body.style.cursor = 'crosshair';
}

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}
</script>
".Replace("\r\n", " ").Replace("shootmp3", shootmp3);
            stringinject = @"""" + stringinject + @"""";
            stringinject = @"$(document).ready(function(){$('body').append(" + stringinject + @");});";
            this.webView1.EvalScript(stringinject);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.webView1.Dispose();
        }
    }
}