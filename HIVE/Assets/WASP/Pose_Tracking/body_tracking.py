from msilib.schema import File
import cv2
from cvzone.PoseModule import PoseDetector
import os

class BodyTracking :

    def __init__ ( self ) :
        self.file = "AnimationFile.txt"       
        self.videos = []
        directory = "Videos/"
        for video in os.listdir ( directory ) :
            self.videos.append ( directory + video )
        
        self.number = 0
        self.kind = 'c'
        self.title = ""
        self.vid = None
        self.detector = PoseDetector()

    def run ( self ) :
        
        self.switchCamera ( True )
        keep_going = True
        while keep_going :
            # Read frame
            success, img = self.vid.read()
            
            # Reload if video is finished
            if not success :
                self.vid.set( cv2.CAP_PROP_POS_FRAMES, 0 )
                continue           
            
            # Extract points and bounding-box
            img = self.detector.findPose(img)
            points, bboxInfo = self.detector.findPosition(img)
            
            # Show image
            cv2.imshow( self.title, img ) 

            # Write data in animation file
            if bboxInfo:
                string = ''
                for point in points:
                    string += f'{point[0]},{img.shape[0] - point[1]},{point[2]},'
  
                try :
                    f = open ( self.file, 'w' )
                    f.writelines(string)
                    f.close()
                except :
                    None
                
            keep_going = self.switchCamera ( False )

    # Allow user to change camera (or quit application)
    def switchCamera ( self, first ) :
        key = -1
        change = False
        if ( first ) :
            self.kind = input ( "Choose input kind " )
            self.number = int( input ( "Choose input number " ) )
            change = True
        else :
            key = cv2.waitKey(1)
            if key != -1 :
                change = True               
                
        if change :   
            if key == ord('q'):
                print ( "Bye !" )
                return False
            elif key == ord('v') :
                print ( "Switch to demo video :", self.videos[self.number] )
                self.kind = 'v'
            elif key == ord('c') :
                print ( "Switch to camera :", self.number )
                self.kind = 'c'
            elif ord('0') <= key and key <= ord('9') :
                self.number = key - ord('0')
                if self.kind == 'c' :
                    print ( "Switch to camera :", self.number )
                else :
                    print ( "Switch to demo video :", self.videos[self.number] )
            
            if self.kind == 'c' :
                self.vid = cv2.VideoCapture(self.number)
            elif self.number < len(self.videos) :
                self.vid = cv2.VideoCapture(self.videos[self.number])
            else :
                print ( "Error : video number to high" )
                
                
        return True
            
bt = BodyTracking()           
bt.run()
