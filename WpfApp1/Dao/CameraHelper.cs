using System;
using AForge.Controls;
using System.Windows;
using System.IO;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;

namespace WpfApp1
{
    class CameraHelper
    {
        private CameraHelper() {

        }
        private static FilterInfoCollection _cameraDevices;
        private static VideoCaptureDevice div = null;
        private static VideoSourcePlayer sourcePlayer = new VideoSourcePlayer();
        private static bool _isDisplay = false;
        //指示_isDisplay设置为true后，是否设置了其他的sourcePlayer，若未设置则_isDisplay重设为false
        private static bool isSet = false;

        /// <summary>
        /// 获取或设置摄像头设备，无设备为null
        /// </summary>
        public static FilterInfoCollection CameraDevices
        {
            get
            {
                return _cameraDevices;
            }
            set
            {
                _cameraDevices = value;
            }
        }
        /// <summary>
        /// 指示是否显示摄像头视频画面
        /// 默认false
        /// </summary>
        public static bool IsDisplay
        {
            get { return _isDisplay; }
            set { _isDisplay = value; }
        }
        /// <summary>
        /// 获取或设置VideoSourcePlayer控件，
        /// 只有当IsDisplay设置为true时，该属性才可以设置成功
        /// </summary>
        public static VideoSourcePlayer SourcePlayer
        {
            get { return sourcePlayer; }
            set
            {
                if (_isDisplay)
                {
                    sourcePlayer = value;
                    isSet = true;
                }

            }
        }
        public static bool CameraInit(VideoSourcePlayer player) {
            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;
            CameraHelper.UpdateCameraDevices();
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
                return true;
            }
            else
            {
                MessageBox.Show("你的电脑没有找到摄像头，请更换电脑");
                return false;
            }
        }

        /// <summary>
        /// 更新摄像头设备信息
        /// </summary>
        public static void UpdateCameraDevices()
        {
            _cameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }
        /// <summary>
        /// 设置使用的摄像头设备
        /// </summary>
        /// <param name="index">设备在CameraDevices中的索引</param>
        /// <returns><see cref="bool"/></returns>
        public static bool SetCameraDevice(int index)
        {
            if (!isSet) _isDisplay = false;
            //无设备，返回false
            if (_cameraDevices.Count <= 0 || index < 0) return false;
            if (index > _cameraDevices.Count - 1) return false;
            // 设定初始视频设备
            div = new VideoCaptureDevice(_cameraDevices[index].MonikerString);
            sourcePlayer.VideoSource = div;
            div.Start();
            sourcePlayer.Start();
            return true;
        }
        /// <summary>
        /// 截取一帧图像并保存aaa
        /// </summary>

        public static byte[]   CaptureImage()
        {
            if (sourcePlayer.VideoSource == null) return null;
      
            try
            {
     
                Image bitmap = sourcePlayer.GetCurrentVideoFrame();
           
               MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
              return ms.ToArray();

           
           
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return null;
            }
            
        }
        /// <summary>
        /// 关闭摄像头设备
        /// </summary>
        public static void CloseDevice()
        {
            if (div != null && div.IsRunning)
            {
                sourcePlayer.Stop();
                div.SignalToStop();
                div = null;
                _cameraDevices = null;
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

   
    }
}

