# Learning Academy — project guide

Open `Assets/Scenes/MainMenu.unity` and press Play. Choose a learner, enter the academy, inspect image frames, read PDF frames, and answer the related quiz. Each frame opens exactly one content type. Movement pauses while reading or answering.

The welcome UI is saved in the MainMenu scene under **Academy Welcome UI → Academy menu**. Edit its Text, Image and RectTransform components directly in Unity. Previous, Next and Enter Academy have persistent Inspector button events. Legacy canvases were removed. `ChildSelection` uses this saved UI; it does not generate another menu in Play Mode. **Tools → Academy → Save menu UI to scene** regenerates the baseline layout and replaces existing menu canvases, so normal visual adjustments should be made in the hierarchy instead.

## Presentation flow

1. Show the learner selection screen.
2. Enter the classroom: desks, reading shelves, plants, ceiling lighting and wall-mounted learning stations.
3. Click an image on either side wall and zoom in.
4. Close it and click that subject's PDF frame; demonstrate page navigation.
5. Close the PDF and click its quiz on the front wall. Explain feedback, source references, score and retry.
6. Show the JSON catalog and explain that question counts and subjects are data, not hardcoded game logic.

Controls: WASD/arrows to move, hold right mouse and drag to look, click frames to interact. On-screen direction buttons and the look pad support touch. Escape closes the current learning overlay. The UI is designed for landscape screens, with a 1280 × 800 reference layout.

## Folder map

| Folder | Responsibility |
| --- | --- |
| `Assets/Scripts/Core` | Scene entry and learner selection |
| `Assets/Scripts/Content` | Serializable lessons, catalog validation, external image loading |
| `Assets/Scripts/Environment` | Room geometry, furniture and clickable frames |
| `Assets/Scripts/Gameplay` | Player movement, camera, quiz state and room coordination |
| `Assets/Scripts/UI` | Shared visual theme, welcome screen and material reader |
| `Assets/Editor/StudyRoomBuild.cs` | Rebuild the saved classroom and run integration checks |
| `Assets/Scenes` | MainMenu and StudyRoom |
| `Assets/Resources/Lessons` | Bundled JSON, teaching images and PDF page previews |
| `Assets/StreamingAssets/Lessons` | Original source PDFs |
| `Assets/URPDefaultResources` | Mobile/PC renderer settings |
| `Tools` | Offline learning-material generation and PDF import |
| `Archive/Prototype` | Retired prototype files, excluded from Unity imports |

Vendor folders (ToonKids, ThirdPerson Control, TextMesh Pro) remain separate. Character models/materials and controller dependencies must remain in the project. Preserve `.meta` files when moving assets: their GUIDs keep scene references intact.

## Explain the code

`ChildSelection` saves the chosen index. `GameManager` activates that learner and initializes `StudyRoom`. `StudyRoomEnvironment` builds the room and lays out two learning packs per room page. `StudyFrame` forwards a click with a subject and one type. `StudyRoom` routes this to the quiz or `StudyMaterialViewer`. `StudyLessonCatalog` validates the data. `AcademyUI` owns shared colors and button styling.

The environment class is split into shell/frame construction and academy furniture. Both files are one partial class. This keeps the procedural geometry easy to follow without adding an asset framework.

## Change or add learning packs

Edit `Assets/Resources/Lessons/catalog.json`. Each lesson contains:

```json
{
  "subject": "Science",
  "title": "Plants and sunlight",
  "imageResource": "Lessons/science-image",
  "pdfPages": ["Lessons/science-page-1", "Lessons/science-page-2"],
  "pdfFile": "Lessons/science.pdf",
  "questions": [
    {
      "prompt": "What gives plants energy to grow?",
      "options": ["Sunlight", "Plastic", "Sand", "Glass"],
      "correct": 0,
      "source": "Science PDF, page 1: sunlight"
    }
  ]
}
```

Add this object to the top-level `lessons` array. Supply its matching image and page files; the example paths are illustrative, not bundled Science assets. Subject names must be unique. Questions need four nonempty answers, a zero-based correct index and a source. Any positive question count is supported. More than two subjects automatically enables **Next subjects** in the room. Every subject gets separate image, PDF and quiz frames.

Use **Tools → Academy → Rebuild classroom** to update saved previews after changing bundled content. Runtime frames also refresh from the catalog when entering the room.

## Supply content after a build

Place `catalog.json` in `Application.persistentDataPath/Academy/` and put the referenced PNG files underneath it, for example `Academy/Lessons/science-image.png`. Restart the room to load the pack. Remove the external catalog to return to the bundled lessons. Invalid JSON/data falls back to bundled lessons with a warning. Missing image files show an unavailable-page message. External images must remain inside the Academy folder.

This is a local content-pack workflow; it does not require a backend. It is not a live upload dashboard. For a Windows build, Unity's persistent data directory normally lives under `%USERPROFILE%/AppData/LocalLow/<CompanyName>/<ProductName>`.

PDF pages are rendered to PNGs **during authoring**, then read inside the game. Supplying only a raw PDF is not sufficient. Import a new PDF on Windows using:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Tools/ImportStudyPdf.ps1 -PdfPath "C:/Lessons/science.pdf" -OutputDirectory "Assets/Resources/Lessons" -Prefix science
```

List the resulting `science-page-1`, etc. in `pdfPages`, copy the original PDF into StreamingAssets/Lessons, and update `pdfFile`. For an external pack, use its `Academy/Lessons` directory as the output. The PDF importer uses the Windows native PDF renderer; the game displays the resulting images on all supported Unity targets. This avoids an additional licensed PDF runtime plugin.

## Rendering and references

Unity 6000.3.8f1 with URP 17.3.0. The academy shell and furniture use URP Lit materials. PC settings use 4× MSAA and per-pixel additional lighting. Learning illustrations use the project's picture shader to keep text legible. The environment is original procedural geometry; no paid school environment pack was downloaded. Existing ToonKids models are project-provided assets; retain their original licensing documentation.

- [Unity 6.3: Lighting in URP](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/lighting-landing.html)
- [Unity 6.3: URP Lit shader](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/lit-shader.html)
- Interaction reference supplied by the project owner: [PMY on Google Play](https://play.google.com/store/apps/details?id=com.nbmetaverse.xanaPMY)

This is a stylized academy interior. The current work does not include an exterior campus, multiplayer, a content-management server or a photorealistic purchased environment.

## Validation

`AR.StudyRoomBuild.BuildAndValidate` runs inside an isolated Unity project copy under Library. It regenerates the saved environment, checks wall/border raycasts, opens image and PDF frames, verifies page navigation/zoom/pause, and completes both demo quizzes including duplicate-answer protection and retry. Captures are stored in `Docs/StudyRoom` after inspection. Device-specific Android/iOS performance still requires testing on the target device.
