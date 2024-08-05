const { app, BrowserWindow } = require('electron');
const path = require('path');

function createWindow () {
  const mainWindow = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, 'src/preload.js'), // Correct path to preload.js
      nodeIntegration: true, // Secure approach: Disable nodeIntegration
      contextIsolation: false, // Enable contextIsolation for security
    }
  });

  mainWindow.loadFile('src/index.html'); // Ensure the correct path to your HTML file
}

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
