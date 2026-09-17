# Study room

Open `Assets/Scenes/MainMenu.unity`, select a child, and press Play. It loads the saved `Assets/Scenes/StudyRoom.unity` scene. You can also open that scene directly: the complete room and frames are visible in the editor before Play Mode.

The room has six gold frames across three walls: one image, one PDF and one quiz frame for each of Math and English. The left wall contains Math learning material; the right wall contains English material; the front wall has the related quizzes. Frames display the actual image or first PDF page. Clicking the picture or gold border opens its content.

- Move with WASD, arrow keys, or the on-screen direction buttons.
- Hold the right mouse button to look around, or drag the lower-right touch look pad. The third-person camera stays inside the walls.
- Click an IMAGE frame to view an illustrated lesson in the room.
- Click a PDF frame to read the two-page lesson pack. Use Previous/Next, +/-, Fit, and drag-to-pan after zooming.
- Each frame opens only its own content type: image frames show only an image, PDF frames show only the PDF reader, and quiz frames show only the quiz. Close the viewer to return to the room and select another frame.
- Quiz frames remain freely accessible; reading is encouraged, not a prerequisite.
- Choose one answer, read feedback, then press Next question.
- Results show the score; Try again restarts that subject. X or Escape returns to the room.
- Personal bests are saved separately for each subject using PlayerPrefs.
- Characters returns to the existing character selection screen.

Learning content is configured in `Assets/Resources/Lessons/catalog.json`. Each lesson links an image, ordered PDF page resources, the original PDF file, and five quiz questions. Each question includes four choices, a zero-based correct answer, and the source image/PDF page, which is shown after answering. Update questions and sources when changing the material; questions are authored from the learning content, not automatically generated from arbitrary files. Both the reader and quiz pause movement and camera input.

Included sample packs:

- Math image: 1 + 1 and a triangle's three sides. PDF page 1: 3 + 2 and 2 + 4; page 2: 5 - 2.
- English image: A/Apple, B/Ball, C/Cat and letter order. PDF page 1: big/small; page 2: spelling CAT and the colour blue.

The original PDFs are bundled in `Assets/StreamingAssets/Lessons`. The in-game reader uses page PNGs rendered from those PDFs during authoring, so it works offline without an external PDF app or a runtime PDF plugin. To replace a PDF, copy the new file into StreamingAssets and render its pages on Windows:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Tools/ImportStudyPdf.ps1 -PdfPath Assets/StreamingAssets/Lessons/math.pdf -OutputDirectory Assets/Resources/Lessons -Prefix math
```

Set the catalog's `pdfPages` to the resulting resources in reading order (omit `Assets/Resources/` and `.png`). Replace image lessons with PNGs and update `imageResource`. Rebuild the gallery to update its wall thumbnails and preserve original image proportions. The importer uses Windows' native PDF renderer; it does not run in the shipped game. It does not remove old page assets when a replacement PDF has fewer pages.

The sample artwork and documents are reproducible with `Tools/GenerateStudyLessons.cs`, using PowerShell `Add-Type -Path Tools/GenerateStudyLessons.cs -ReferencedAssemblies System.Drawing`, then `[GenerateStudyLessons]::Build((Get-Location).Path)`. Run the PDF importer for both packs afterward to refresh the page previews from the generated PDFs.

The saved environment prefab is `Assets/Resources/StudyGallery.prefab`; its materials are in `Assets/Resources/StudyGalleryMaterials`. Edit the prefab or scene normally in Unity. `StudyRoomEnvironment.cs` defines the procedural source geometry. The editor command **Tools > Study Room > Build gallery scene** rebuilds the generated scene and prefab from that source, replacing edits to those generated assets. The original demo scene remains available.

`StudyRoomSurface.shader` uses an untagged render pass so the room renders in the project's Built-in renderer; the former SRP-only pass caused invisible walls and frames.

Validation: compiled and ran the scene in Unity 6000.3.8f1 in an isolated copy. Checks passed for six frame/border raycasts, bundled images and PDFs, image display, original texture proportions, PDF next/previous and page boundaries, zoom, reader pause state, both quizzes, duplicate-answer protection, results/retry/close. Windows' native PDF parser loaded both two-page documents, and the reader pages were rendered from those files. Actual Unity wall-frame and reader renders were visually inspected. Android device testing remains outstanding.

Verified previews: [learning frames](Docs/StudyRoom/learning-frames.png), [image reader](Docs/StudyRoom/image-reader.png), [PDF reader](Docs/StudyRoom/pdf-reader.png), [quiz](Docs/StudyRoom/quiz.png). The check summary is in `Docs/StudyRoom/validation.txt`. The batch verification entry point is `AR.StudyRoomBuild.BuildAndValidate`; run it in an isolated copy because it generates assets and uses test scores.

Manual checks: enter with two different characters; move against walls and furniture; complete both quizzes with mixed answers; confirm rapid clicks score only once; close a quiz midway; retry; return to character selection; restart and check saved bests; test touch controls and both screen orientations.

Visual reference: https://play.google.com/store/apps/details?id=com.nbmetaverse.xanaPMY&hl=en
# Current academy version

See [the Academy project guide](Docs/ACADEMY_GUIDE.md) for the current folder structure, URP environment, UI, dynamic packs and presentation instructions. The notes below describe the earlier gallery prototype; the academy guide takes precedence.
