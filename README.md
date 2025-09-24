# Room Wizard & Model Navigator – Revit Add-ins  

## 📌 Overview  
This repository provides two powerful **Revit add-ins** designed to streamline BIM workflows:  

- **Room Wizard** – a tool to quickly generate room section boxes and custom floors with user-defined thickness and materials.  
- **Model Navigator** – a model management tool to update, select, delete, and recolor elements for better coordination and visualization.  

Both plugins are built using the **Revit API (C#)** and help automate repetitive tasks, improve model accuracy, and speed up project delivery.  

---

## 🚀 Features  

### Room Wizard  
🔹 **Room List Browser** – easily browse and select rooms in the project.  
🔹 **Section Box Generator** – isolate selected rooms in a dedicated 3D view.  
🔹 **Floor Creator** – automatically generate floors for selected rooms with custom thickness and material.  
🔹 **Offset & Precision Control** – define base offsets and create LOD-ready geometry.  
🔹 Ensures **non-overlapping floor generation** with update logic.  

### Model Navigator  
🔹 **Element Update** – modify parameters of selected elements directly.  
🔹 **Quick Select** – filter and highlight elements for review.  
🔹 **Batch Delete** – safely remove unwanted elements.  
🔹 **Color Override** – apply temporary colors to elements for better visualization and coordination.  

---

## 🛠️ Installation  

1. Download the `.addin` manifest and `.dll` files from the release package.  
2. Place them in your Revit Add-ins folder, e.g.:  

   ```plaintext
   C:\ProgramData\Autodesk\Revit\Addins\<RevitVersion>\
   Restart Revit – the plugins will appear in the **Add-Ins tab**.  
---

## 📖 Usage  

### Room Wizard  
1. Open your Revit model and launch **Room Wizard** from the Add-ins tab.  
2. Select a room from the list.  
3. Click **Create Section Box** to isolate the room.  
4. Define floor material and thickness.  
5. Click **Generate Floor** – a new floor will be created or an existing one updated.  

### Model Navigator  
1. Launch **Model Navigator** from the Add-ins tab.  
2. Use the tools to:  
   - **Update** element parameters.  
   - **Select** elements by filter or ID.  
   - **Delete** unwanted elements.  
   - **Change Color** for visual checks and coordination.  

---

## 📋 Requirements  
- Autodesk **Revit 2020 or later**  
- **.NET Framework / .NET Core** compatible with Revit API  
- **Windows 10/11**  

---

## 🤝 Contributing  
Contributions, feature requests, and issues are welcome! Please open a pull request or issue in this repository.  

---

## 📜 License  
This project is licensed under the **MIT License** – feel free to use and adapt. 
