
from real_time_driver_state_detection import real_time_driver_state_detection_appl
from Stress_Detector_master import test2
from asyncio.windows_events import NULL
from asyncore import write
from fileinput import filename
import os
from os import listdir
from os.path import isfile, join
import json
from time import time
from xmlrpc.client import MAXINT
import cv2
from deepface import DeepFace
import ctypes
import mimetypes
import tkinter as tk
import tkinter.font as tkFont
from tkinter import filedialog
from tkinter import *
# drowsiness
from scipy.spatial import distance
from imutils import face_utils
import imutils
import dlib
# gesture
import numpy as np
import mediapipe as mp
import tensorflow as tf
from tensorflow.keras.models import load_model
# realtimedriverstate
# import real_time_driver_state_detection as Realtime
# from real_time_driver_state_detection as Realtime
from real_time_driver_state_detection.real_time_driver_state_detection_appl import *
# import real_time_driver_state_detection.real_time_driver_state_detection_appl as Realtime
# import sys
# sys.path.insert(0, '/real_time_driver_state_detection/')

cwd = os.getcwd()
mimetypes.init()


def framesPerSecond(file: str):

    print("hier" + file + "zuende")
    cap = cv2.VideoCapture(file)
    # fps = cap.get(cv2.CAP_PROP_FPS)

    (major_ver, minor_ver, subminor_ver) = (cv2.__version__).split('.')

    if int(major_ver) < 3:
        fps = cap.get(cv2.cv.CV_CAP_PROP_FPS)

        print(
            "Frames per second using video.get(cv2.cv.CV_CAP_PROP_FPS): {0}".format(fps))
    else:
        fps = cap.get(cv2.CAP_PROP_FPS)
        print(
            "Frames per second using video.get(cv2.CAP_PROP_FPS) : {0}".format(fps))
    # frame_count = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    # duration = frame_count/fps
    return fps


def browseFiles():
    return filedialog.askopenfilename(initialdir=cwd,
                                      title="Select a File",
                                      filetypes=(("usable files",
                                                  "*.mp4*"),
                                                 ("usable files",
                                                  "*.avi*"),
                                                 ("all files",
                                                  "*.*"),
                                                 ))

    # Change label contents
#     label_file_explorer.configure(text="File Opened: "+filename)


# window = Tk()


# label_file_explorer = Label(window,
#                             text="File Explorer using Tkinter",
#                             width=100, height=4,
#                             fg="blue")


def framesOfVideo(file: str):
    """
    Returns the number of frames in a video file.

    :param file: the path to the video file
    :return: the number of frames in the video
    """
    cap = cv2.VideoCapture(file)
    length = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    print("Länge des videos in frames" + str(length))
    return length


def splitVideoInFrames(videoFile: str, outDir: str):
    """
    Splits the given video file into its frames and saves each frame as a jpg file.

    :param videoFile: the path to the video file
    :param outDir: the path to the directory the indiviual frames should be stored in
    """
    vidcap = cv2.VideoCapture(videoFile)
    success, image = vidcap.read()
    count = 0
    while success:
        cv2.imwrite(outDir + "/frame%d.jpg" %
                    count, image)     # save frame as JPEG file
        success, image = vidcap.read()
        # print('Read a new frame: ', success)
        # fileName = "frame%d" % count
        # print(fileName)
        count += 1


def mergeJsonFiles(dir: str, outDir: str, name: str, fps: float):
    """
    Merges all json files in the given directory into one file.

    :param dir: the path to the directory containing the json files
    :param outDir: the path to the directory the combined json file should be stored in
    :param name: the name of the generated json file
    """
    # os.chdir(dir)
    timestamp = 0
    file = [f for f in listdir(
        dir) if isfile(join(dir, f))]
    result = list()
    for f1 in file:
        with open(dir + "/" + f1, "r") as infile:
            singleJson = json.load(infile)
            timestamp += 1/fps
            singleJson["timestamp"] = timestamp
            result.append(singleJson)

    with open(outDir + "/" + name + ".json", "w") as output_file:
        json.dump(result, output_file)


def openpose(file: str, filepath: str):
    """
    Applies the openpose algorithm to the given image or video.

    :param file: the name of the image or video to which the algorithm should be applied
    """

    os.chdir('Openpose2/openpose')
    stream = os.popen(
        "build\\x64\Release\OpenPoseDemo.exe --video " + filepath + file + " --write_json ../../output/openpose/tmp --render_pose 0 --display 0")
    output = stream.read()
    output
    os.chdir(cwd)
    completePath = filepath+file
    filewithout = completePath.split('"')[1]
    fps = framesPerSecond(filewithout)
    # merge the json files generated for each frame
    mergeJsonFiles("output/openpose/tmp",
                   "output/openpose", file.split(".")[0], fps)

    # remove the old json files
    jsons = [f for f in listdir(
        "output/openpose/tmp") if isfile(join("output/openpose/tmp", f))]
    for j in jsons:
        os.remove("output/openpose/tmp/" + j)


def deepfacePic(file: str, outDir: str, fromVid: bool):
    """
    Applies the deepface algorithm to the given image.

    :param file: the name of the image to which the algorithm should be applied
    :param outDir: the path to the directory the json file should be stored in
    :param fromVid: true if the function is called by the "deepfaceVid()" function, false otherwise
    """
    # models = ["VGG-Face", "Facenet", "Facenet512", "OpenFace",
    #           "DeepFace", "DeepID", "ArcFace", "Dlib", "SFace"]

    # face verification
    # result = DeepFace.verify(img1_path="image_happy.jpg",  model_name=models[1])

    # face recognition
    # df = DeepFace.find(img_path="image_happy.jpg",
    #                    db_path="C:/workspace/my_db", model_name=models[1])

    # obj = DeepFace.analyze(img_path="megan_fox.jpg", actions=[
    #                        'age', 'gender', 'race', 'emotion'])

    if(fromVid):
        obj = DeepFace.analyze(img_path="input/deepface/tmp/" + file, actions=[
            'age', 'gender', 'race', 'emotion'], enforce_detection=False)
    else:
        obj = DeepFace.analyze(img_path="input/deepface/" + file, actions=[
            'age', 'gender', 'race', 'emotion'], enforce_detection=False)

    filename = file.split(".")[0]
    with open(outDir + "/" + filename + ".json", "w") as output_file:
        json.dump(obj, output_file)


def deepfaceVid(file: str):
    """
    Applies the deepface algorithm to the given video.

    :param file: the name of the video to which the algorithm should be applied
    """

    # apply deepface to all images of the video
    splitVideoInFrames(
        file, "input/deepface/tmp")
    images = [f for f in listdir(
        "input/deepface/tmp") if isfile(join("input/deepface/tmp", f))]
    for image in images:
        deepfacePic(image, "output/deepface/tmp", True)

    fixedfile = file
    files = fixedfile.split("/")
    print("test " + fixedfile + " yo")
    fps = framesPerSecond(fixedfile)

    print(fps)

    # merge all json files
    mergeJsonFiles("output/deepface/tmp",
                   "output/deepface", files[len(files)-1], fps)

    # remove the old images and json files
    jsons = [f for f in listdir(
        "output/deepface/tmp") if isfile(join("output/deepface/tmp", f))]
    for image in images:
        os.remove("input/deepface/tmp/" + image)
    for j in jsons:
        os.remove("output/deepface/tmp/" + j)


def yoloPic():
    """
    Applies the yolo algorithm to the given image.
    """
    os.chdir('Yolo2\darknet\\build\darknet\\x64')
    os.system(
        "darknet.exe detector test cfg/coco.data cfg/yolov4.cfg yolov4.weights")
    os.chdir(cwd)


def yoloVid(file: str):
    """
    Applies the yolo algorithm to the given video.

    :param file: the name of the video to which the algorithm should be applied
    """
    print("hier"+file + "zuende")
    os.chdir('Yolo2\darknet\\build\darknet\\x64')
    test = framesOfVideo(file)
    print(test)

    os.system(
        '"darknet.exe detector demo cfg/coco.data cfg/yolov4.cfg yolov4.weights ' + file + ' >yolo.txt -dont_show"')

    # stream = os.popen(
    #     "darknet.exe detector demo cfg/coco.data cfg/yolov4.cfg yolov4.weights ../../../../../input/yolo/" + file)
    # output = stream.read()
    # print("hier" + output)
    # print("zuende")
    # print("hier anzahl" + str(output.count("Objects")))
    handleYoloFile()
    os.chdir(cwd)


def handleYoloFile():
    os.chdir(cwd)
    os.chdir('Yolo2\darknet\\build\darknet\\x64')

    file = open("yolo.txt")
    filesplitted = file.read().replace(
        "\t", "").replace("\n\n", "\n").split("Objects:")
    result = list()

    index = 0
    for f in filesplitted:

        if(index >= 2):
            result.append(f)
        index += 1

    JsonString = '['
    index3 = 0
    timestamp = 0
    for x in result:
        timestamp += 1/30
        JsonString += '{"timestamp":' + str(timestamp) + ',"Objects":['

        singleFrame = x.split("\n")
        # print(singleFrame)
        index2 = 0
        lengthofsingleFrame = 2
        if(index3 >= len(result)-2):
            lengthofsingleFrame = 3
        for y in singleFrame:
            if(index2 > 0 and index2 == (len(singleFrame)-lengthofsingleFrame)-1):
                singleEntryinFrame = y.split(":")
                if(index2 > 0 and index2 < len(singleFrame)-lengthofsingleFrame):
                    JsonString += '{"type": "' + str(singleEntryinFrame[0]) + '","accuracy": "' + str(
                        singleEntryinFrame[1].replace(" ", "")) + '"}'
                else:
                    JsonString += '{"type": "' + \
                        str(singleEntryinFrame[0]) + '"}'
            else:
                if(index2 > 0 and index2 < len(singleFrame)-lengthofsingleFrame):
                    singleEntryinFrame = y.split(":")
                    if(len(singleEntryinFrame) > 1):
                        JsonString += '{"type": "' + str(singleEntryinFrame[0]) + '","accuracy": "' + str(
                            singleEntryinFrame[1].replace(" ", "")) + '"},'
                    else:
                        JsonString += '{"type": "' + \
                            str(singleEntryinFrame[0]) + '"},'
            index2 += 1
        JsonString += ']'
        if(index3 == len(result)-1):
            JsonString += '}'
        else:
            JsonString += '},'
        index3 += 1
    JsonString += ']'
    # fullJson = json.dumps(JsonString)
    # test = json.loads(JsonString)
    # print(fullJson)
    os.chdir(cwd)
    name = "video"
    f = open("output/yolo" + "/" + name + ".json", "w")
    f.write(JsonString)


def startOpenpose():
    """
    Runs the openpose algorithm on all files located in the input/openpose directory.
    """
    print("Running OpenPose...")
    files = [f for f in listdir(
        "input/openpose") if isfile(join("input/openpose", f))]
    count = 1
    for file in files:
        openpose(file)
        print("{}/{} finished".format(count, len(files)))
        count += 1

    print("OpenPose done!\n")


def startDeepface():
    """
    Runs the deepface algorithm on all files located in the input/deepface directory.
    """
    print("Running DeepFace...")
    files = [f for f in listdir(
        "input/deepface") if isfile(join("input/deepface", f))]
    count = 1
    for file in files:
        mimestart = mimetypes.guess_type(file)[0]
        if mimestart != None:
            mimestart = mimestart.split('/')[0]
            if mimestart == "video":
                deepfaceVid(file)
            if mimestart == "image":
                deepfacePic(file, "output/deepface", False)
        print("{}/{} finished".format(count, len(files)))
        count += 1
    print("DeepFace done!\n")


def startYolo():
    """
    Runs the yolo algorithm on all files located in the input/yolo directory.
    """
    print("Running YOLO...")
    files = [f for f in listdir(
        "input/yolo") if isfile(join("input/yolo", f))]
    count = 1
    for file in files:
        mimestart = mimetypes.guess_type(file)[0]
        if mimestart != None:
            mimestart = mimestart.split('/')[0]
            if mimestart == "video":
                yoloVid(file)
            if mimestart == "image":
                yoloPic()
        print("{}/{} finished".format(count, len(files)))
        count += 1
    print("YOLO done!\n")


def startAll():
    """
    Runs the all the ML algorithms on all files located in the respective directorys.
    """
    startOpenpose()
    startDeepface()
    startYolo()


# startAll()

# startOpenpose()
# startDeepface()
# startYolo()

def startOpenPoseSingle(file, filepath):
    openpose(file, filepath)


def startDeepfaceSingle(file):
    deepfaceVid(file)
    # mimestart = mimetypes.guess_type(file)[0]
    # if mimestart != None:
    #     mimestart = mimestart.split('/')[0]
    #     if mimestart == "video":
    #         deepfaceVid(file)
    #     if mimestart == "image":
    #         deepfacePic(file, "output/deepface", False)


def startYoloSingle(file):
    yoloVid(file)
    # mimestart = mimetypes.guess_type(file)[0]
    # if mimestart != None:
    #     mimestart = mimestart.split('/')[0]
    #     if mimestart == "video":
    #         yoloVid(file)
    #     if mimestart == "image":
    #         yoloPic()


def eye_aspect_ratio(eye):
    A = distance.euclidean(eye[1], eye[5])
    B = distance.euclidean(eye[2], eye[4])
    C = distance.euclidean(eye[0], eye[3])
    ear = (A + B) / (2.0 * C)
    return ear


def drowsiness_detection(file):
    thresh = 0.25
    frame_check = 20
    os.chdir("drowsiness_detection")
    detect = dlib.get_frontal_face_detector()
    # Dat file is the crux of the code
    predict = dlib.shape_predictor(
        "models/shape_predictor_68_face_landmarks.dat")

    (lStart, lEnd) = face_utils.FACIAL_LANDMARKS_68_IDXS["left_eye"]
    (rStart, rEnd) = face_utils.FACIAL_LANDMARKS_68_IDXS["right_eye"]
    print(file)
    cap = cv2.VideoCapture(file)
    print("test")
    flag = 0
    valueArray = []
    while True:
        ret, frame = cap.read()
        if(ret == False):
            break
        frame = imutils.resize(frame, width=750)
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        subjects = detect(gray, 0)
        if(len(subjects) == 0):
            valueArray.append("empty")
        else:
            for subject in subjects:
                shape = predict(gray, subject)
                shape = face_utils.shape_to_np(
                    shape)  # converting to NumPy Array
                leftEye = shape[lStart:lEnd]
                rightEye = shape[rStart:rEnd]
                leftEAR = eye_aspect_ratio(leftEye)
                rightEAR = eye_aspect_ratio(rightEye)
                ear = (leftEAR + rightEAR) / 2.0
                leftEyeHull = cv2.convexHull(leftEye)
                rightEyeHull = cv2.convexHull(rightEye)
                cv2.drawContours(frame, [leftEyeHull], -1, (0, 255, 0), 1)
                cv2.drawContours(frame, [rightEyeHull], -1, (0, 255, 0), 1)
                valueArray.append(ear)
                # if ear < thresh:
                #     flag += 1
                #     print(flag)
                #     if flag >= frame_check:
                #         cv2.putText(frame, "****************ALERT!****************", (10, 30),
                #                     cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                #         cv2.putText(frame, "****************ALERT!****************", (10, 325),
                #                     cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 255), 2)
                #         #print ("Drowsy")
                # else:
                #     flag = 0
        cv2.imshow("Frame", frame)
        key = cv2.waitKey(1) & 0xFF
        if key == ord("q"):
            break
    cv2.destroyAllWindows()
    cap.release()
    createJson(file, "drowsiness_detection",
               "ear", valueArray, "empty", "video")


def gesture_recognition(file):

    # initialize mediapipe
    mpHands = mp.solutions.hands
    hands = mpHands.Hands(max_num_hands=1, min_detection_confidence=0.7)
    mpDraw = mp.solutions.drawing_utils
    os.chdir("gesture_recognition")

    # Load the gesture recognizer model
    model = load_model('mp_hand_gesture')

    # Load class names
    f = open('gesture.names', 'r')
    classNames = f.read().split('\n')
    f.close()
    print(classNames)

    # Initialize the webcam
    cap = cv2.VideoCapture(file)
    valueArr = []
    while True:
        # Read each frame from the webcam
        ret, frame = cap.read()
        if(ret == False):
            break
        x, y, c = frame.shape

        # Flip the frame vertically
        frame = cv2.flip(frame, 1)
        framergb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        # Get hand landmark prediction
        result = hands.process(framergb)

        # print(result)

        className = ''

        # post process the result
        if result.multi_hand_landmarks:
            landmarks = []
            for handslms in result.multi_hand_landmarks:
                for lm in handslms.landmark:
                    # print(id, lm)
                    lmx = int(lm.x * x)
                    lmy = int(lm.y * y)

                    landmarks.append([lmx, lmy])

                # Drawing landmarks on frames
                mpDraw.draw_landmarks(
                    frame, handslms, mpHands.HAND_CONNECTIONS)

                # Predict gesture
                prediction = model.predict([landmarks])
                # print(prediction)
                classID = np.argmax(prediction)
                className = classNames[classID]
                valueArr.append(className)

        # show the prediction on the frame
        cv2.putText(frame, className, (10, 50), cv2.FONT_HERSHEY_SIMPLEX,
                    1, (0, 0, 255), 2, cv2.LINE_AA)

        # Show the final output
        cv2.imshow("Output", frame)

        if cv2.waitKey(1) == ord('q'):
            break

    # np.where(valueArr == "smile", "empty", valueArr)
    print(valueArr)
    createJson(file, "gesture_recognition",
               "gesture", valueArr, "smile", "video")
    # release the webcam and destroy all active windows
    cap.release()

    cv2.destroyAllWindows()


def createJson(file, dir, key, ValueArray, defaultvalue, name):
    print("test")
    jsonString = ''
    thisfps = framesPerSecond(file)
    jsonString += '['
    index = 0
    timestamp = 0
    for value in ValueArray:
        timestamp += 1/thisfps
        if(value == defaultvalue):
            if(index != len(ValueArray)-1):
                jsonString += '{"timestamp":' + \
                    str(timestamp) + '},'
            else:
                jsonString += '{"timestamp":' + \
                    str(timestamp) + '}'
        else:
            if(index != len(ValueArray)-1):
                jsonString += '{"timestamp":' + \
                    str(timestamp) + ',"'+str(key) + '":"' + str(value) + '"},'
            else:
                jsonString += '{"timestamp":' + \
                    str(timestamp) + ',"'+str(key) + '":"' + str(value) + '"}'
        index += 1
    jsonString += ']'
    os.chdir(cwd)
    f = open("output/"+dir + "/" + name + ".json", "w")
    f.write(jsonString)


"""
ui
"""
filepath1 = ""
filepath2 = ""
filepath3 = ""
filepath4 = ""
filepath5 = ""
filepath6 = ""
filepath7 = ""


def button1_command(label):
    print("command1")
    file = browseFiles()
    if(file != ""):
        global filepath1
        filepath1 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button2_command(label):
    print("command2")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath2
        filepath2 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button3_command(label):
    print("command3")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath3
        filepath3 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button4_command(label):
    print("command3")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath4
        filepath4 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button5_command(label):
    print("command3")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath5
        filepath5 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button6_command(label):
    print("command3")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath6
        filepath6 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def button7_command(label):
    print("command3")
    print(label["text"])
    file = browseFiles()
    if(file != ""):
        global filepath7
        filepath7 = file
        files = file.split("/")
        print(file)
        label["text"] = ".../" + \
            files[len(files)-2] + "/" + files[len(files)-1]


def runButton_command():

    # print(filepath1)
    # print(filepath2)
    # print(filepath3)

    if(filepath1 != ""):
        files = filepath1.split("/")
        filestuff = files[len(files)-1]
        filePathToOpen = filepath1.replace(files[len(files)-1], "")
        filePathToOpen = filePathToOpen.replace("/", "\\")
        nPath = '"{}'.format(filePathToOpen)
        filestuff += '"'
        print(nPath)
        startOpenPoseSingle(filestuff, nPath)
    if(filepath2 != ""):
        files = filepath2.split("/")

        thisfile = '"'+filepath2+'"'
        print(thisfile)
        startYoloSingle(thisfile)
    if(filepath3 != ""):
        files = filepath3.split("/")
        startDeepfaceSingle(filepath3)
    if(filepath4 != ""):
        files = filepath4.split("/")
        real_time_driver_state_detection_appl.real_time_driver_state_detection_run(
            filepath4)
    if(filepath5 != ""):
        files = filepath5.split("/")
        drowsiness_detection(filepath5)
    if(filepath6 != ""):
        files = filepath6.split("/")
        gesture_recognition(filepath6)
    if(filepath7 != ""):
        files = filepath7.split("/")
        test2.VideoCamera(filepath7)
    # gesture_recognition("test2_Trim.mp4")
    # drowsiness_detection("test1_Trim.mp4")
    # os.chdir("real_time_driver_state_detection")
    # real_time_driver_state_detection_appl.real_time_driver_state_detection_run()
    # test2.VideoCamera("test3_Trim_Trim.mp4")
    ctypes.windll.user32.MessageBoxW(
        0, "All Algorithms are done!", "Done!", 0x1000)


class App:

    def __init__(self, root):
        # setting title
        root.title("Machine Learning Algortihm to JSON Converter")
        # setting window size
        width = 600
        height = 800
        screenwidth = root.winfo_screenwidth()
        screenheight = root.winfo_screenheight()
        alignstr = '%dx%d+%d+%d' % (width, height,
                                    (screenwidth - width) / 2, (screenheight - height) / 2)
        root.geometry(alignstr)
        root.resizable(width=False, height=False)

        GLabel_838 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        GLabel_838["font"] = ft
        GLabel_838["fg"] = "#000000"
        GLabel_838["justify"] = "center"
        GLabel_838["text"] = "Openpose"
        GLabel_838.place(x=50, y=130, width=105, height=37)

        GLabel_236 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        GLabel_236["font"] = ft
        GLabel_236["fg"] = "#000000"
        GLabel_236["justify"] = "center"
        GLabel_236["text"] = "YOLO"
        GLabel_236.place(x=50, y=190, width=105, height=37)

        GLabel_570 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        GLabel_570["font"] = ft
        GLabel_570["fg"] = "#000000"
        GLabel_570["justify"] = "center"
        GLabel_570["text"] = "Deepface"
        GLabel_570.place(x=50, y=250, width=103, height=35)

        nameofml1 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        nameofml1["font"] = ft
        nameofml1["fg"] = "#000000"
        nameofml1["justify"] = "center"
        nameofml1["text"] = "Driver-State"
        nameofml1.place(x=50, y=70, width=103, height=35)

        nameofml2 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        nameofml2["font"] = ft
        nameofml2["fg"] = "#000000"
        nameofml2["justify"] = "center"
        nameofml2["text"] = "Drowsiness"
        nameofml2.place(x=50, y=310, width=103, height=35)

        nameofml3 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        nameofml3["font"] = ft
        nameofml3["fg"] = "#000000"
        nameofml3["justify"] = "center"
        nameofml3["text"] = "Gesture"
        nameofml3.place(x=50, y=370, width=103, height=35)

        nameofml4 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        nameofml4["font"] = ft
        nameofml4["fg"] = "#000000"
        nameofml4["justify"] = "center"
        nameofml4["text"] = "Stress"
        nameofml4.place(x=50, y=430, width=103, height=35)

        label1 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label1["font"] = ft
        label1["fg"] = "#000000"
        label1["justify"] = "center"
        label1["text"] = "filepath"
        label1.place(x=190, y=130, width=105, height=37)

        label2 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label2["font"] = ft
        label2["fg"] = "#000000"
        label2["justify"] = "center"
        label2["text"] = "filepath"
        label2.place(x=190, y=190, width=105, height=37)

        label3 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label3["font"] = ft
        label3["fg"] = "#000000"
        label3["justify"] = "center"
        label3["text"] = "filepath"
        label3.place(x=190, y=250, width=105, height=37)

        label4 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label4["font"] = ft
        label4["fg"] = "#000000"
        label4["justify"] = "center"
        label4["text"] = "filepath"
        label4.place(x=190, y=70, width=105, height=37)

        label5 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label5["font"] = ft
        label5["fg"] = "#000000"
        label5["justify"] = "center"
        label5["text"] = "filepath"
        label5.place(x=190, y=310, width=105, height=37)

        label6 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label6["font"] = ft
        label6["fg"] = "#000000"
        label6["justify"] = "center"
        label6["text"] = "filepath"
        label6.place(x=190, y=370, width=105, height=37)

        label7 = tk.Label(root)
        ft = tkFont.Font(family='Times', size=10)
        label7["font"] = ft
        label7["fg"] = "#000000"
        label7["justify"] = "center"
        label7["text"] = "filepath"
        label7.place(x=190, y=430, width=105, height=37)

        button3 = tk.Button(root)
        button3["bg"] = "#0c1415"
        ft = tkFont.Font(family='Times', size=10)
        button3["font"] = ft
        button3["fg"] = "#ffffff"
        button3["justify"] = "center"
        button3["text"] = "Add File"
        button3.place(x=340, y=250, width=108, height=33)
        button3["command"] = lambda: button3_command(label3)

        button2 = tk.Button(root)
        button2["bg"] = "#0c1415"
        ft = tkFont.Font(family='Times', size=10)
        button2["font"] = ft
        button2["fg"] = "#ffffff"
        button2["justify"] = "center"
        button2["text"] = "Add File"
        button2.place(x=340, y=190, width=108, height=33)
        button2["command"] = lambda: button2_command(label2)

        button1 = tk.Button(root)
        button1["bg"] = "#0e1819"
        ft = tkFont.Font(family='Times', size=10)
        button1["font"] = ft
        button1["fg"] = "#ffffff"
        button1["justify"] = "center"
        button1["text"] = "Add File"
        button1.place(x=340, y=130, width=108, height=33)
        button1["command"] = lambda: button1_command(label1)

        button4 = tk.Button(root)
        button4["bg"] = "#0e1819"
        ft = tkFont.Font(family='Times', size=10)
        button4["font"] = ft
        button4["fg"] = "#ffffff"
        button4["justify"] = "center"
        button4["text"] = "Add File"
        button4.place(x=340, y=70, width=108, height=33)
        button4["command"] = lambda: button4_command(label4)

        button5 = tk.Button(root)
        button5["bg"] = "#0e1819"
        ft = tkFont.Font(family='Times', size=10)
        button5["font"] = ft
        button5["fg"] = "#ffffff"
        button5["justify"] = "center"
        button5["text"] = "Add File"
        button5.place(x=340, y=310, width=108, height=33)
        button5["command"] = lambda: button5_command(label5)

        button6 = tk.Button(root)
        button6["bg"] = "#0e1819"
        ft = tkFont.Font(family='Times', size=10)
        button6["font"] = ft
        button6["fg"] = "#ffffff"
        button6["justify"] = "center"
        button6["text"] = "Add File"
        button6.place(x=340, y=370, width=108, height=33)
        button6["command"] = lambda: button6_command(label6)

        button7 = tk.Button(root)
        button7["bg"] = "#0e1819"
        ft = tkFont.Font(family='Times', size=10)
        button7["font"] = ft
        button7["fg"] = "#ffffff"
        button7["justify"] = "center"
        button7["text"] = "Add File"
        button7.place(x=340, y=430, width=108, height=33)
        button7["command"] = lambda: button7_command(label7)

        runButton = tk.Button(root)
        runButton["bg"] = "#3b7b20"
        ft = tkFont.Font(family='Times', size=10)
        runButton["font"] = ft
        runButton["fg"] = "#ffffff"
        runButton["justify"] = "center"
        runButton["text"] = "Run"
        runButton.place(x=240, y=750, width=111, height=39)
        runButton["command"] = lambda: runButton_command()


if __name__ == "__main__":
    root = tk.Tk()
    app = App(root)
    root.mainloop()
