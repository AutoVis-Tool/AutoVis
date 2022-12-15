import os
from os import listdir
from os.path import isfile, join
import json
import cv2
from deepface import DeepFace


def lengthofVideo(file):
    cap = cv2.VideoCapture(file)
    length = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    return length


def splitvideoIntoFrames(file):
    vidcap = cv2.VideoCapture(file)
    success, image = vidcap.read()
    count = 0
    while success:
        cv2.imwrite("DeepfaceData/DeepfacePicture/frame%d.jpg" %
                    count, image)     # save frame as JPEG file
        success, image = vidcap.read()
        print('Read a new frame: ', success)
        fileName = "frame%d" % count
        print(fileName)
        deepface(fileName)
        count += 1


def openpose(file):
    cwd = os.getcwd()
    os.chdir('openPose2/openpose')
    # os.system("build\\x64\Release\OpenPoseDemo.exe --video examples/media/video.avi")
    # os.system("build\\x64\Release\OpenPoseDemo.exe")

    # os.system('openpose\openposetest.py')
    # exec(open("./openpose/openposetest.py").read())
    stream = os.popen(
        "build\\x64\Release\OpenPoseDemo.exe --video examples/media/" + file + " --write_json json")
    output = stream.read()
    output


def deepface(file):

    print(file)
    models = ["VGG-Face", "Facenet", "Facenet512", "OpenFace",
              "DeepFace", "DeepID", "ArcFace", "Dlib", "SFace"]

    # face verification
    # result = DeepFace.verify(img1_path="image_happy.jpg",  model_name=models[1])

    # face recognition
    # df = DeepFace.find(img_path="image_happy.jpg",
    #                    db_path="C:/workspace/my_db", model_name=models[1])

    # obj = DeepFace.analyze(img_path="megan_fox.jpg", actions=[
    #                        'age', 'gender', 'race', 'emotion'])

    obj = DeepFace.analyze(img_path="DeepfaceData/DeepfacePicture/"+file + ".jpg", actions=[
        'age', 'gender', 'race', 'emotion'], enforce_detection=False)

    print(obj)
    with open("DeepfaceData/DeepfaceJson/" + file + ".json", "w") as output_file:
        json.dump(obj, output_file)
    # print(result)


def yolopic():
    os.chdir('Yolo2\darknet\\build\darknet\\x64')
    os.system(
        "darknet.exe detector test cfg/coco.data cfg/yolov4.cfg yolov4.weights")


def yolovid():

    os.chdir('Yolo2\darknet\\build\darknet\\x64')
    # os.system(
    # "darknet.exe detector demo cfg/coco.data cfg/yolov4.cfg yolov4.weights video.avi")
    # os.system(
    #     "darknet.exe detector demo cfg/coco.data cfg/yolov4.cfg yolov4.weights video.avi -i 0 -ext_output > result.txt")
    test = lengthofVideo("video.avi")
    print(test)
    stream = os.popen(
        "darknet.exe detector demo cfg/coco.data cfg/yolov4.cfg yolov4.weights video.avi")
    output = stream.read()
    print(output)
    print(output.count("Objects"))


def combineVideosToOne():
    files = [f for f in listdir(
        "examples/media/") if isfile(join("examples/media/", f))]
    for file in files:
        openpose(file)


def merge_JsonFiles():
    os.chdir('openPose2/openpose')
    file = [f for f in listdir(
        "json") if isfile(join("json", f))]
    result = list()
    for f1 in file:
        with open("json/"+f1, "r") as infile:
            result.append(json.load(infile))

    with open("merge.json", "w") as output_file:
        json.dump(result, output_file)


# merge_JsonFiles()
# openpose("video.avi")
# deepface()
splitvideoIntoFrames("video.avi")
# yolopic()
# yolovid()
