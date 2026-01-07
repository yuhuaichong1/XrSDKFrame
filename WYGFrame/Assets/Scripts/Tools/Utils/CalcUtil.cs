using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XrCode
{
    /// <summary>
    /// 计算帮助类
    /// </summary>
    public static class CalcUtil
    {
        public const float NearFactor = 1f;
        private static Vector3 temp;

        /// <summary>
        /// 数组转Vector3
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 FloatArrayToVertor3(float[] vector)
        {
            return new Vector3(vector[0], vector[1], vector[2]);
        }

        public static float[] Vector3ToFloatArray(Vector3 vector)
        {
            return new float[] { vector.x, vector.y, vector.z };
        }

        public static bool MoveTowardsXZ(Vector3 Pos, Vector3 Target, float MoveDistance, out Vector3 NewPos)
        {
            if (MoveDistance < PosDistanceXZ(Pos, Target))
            {
                temp = Vector3.Normalize(Target - Pos);
                NewPos = Pos + ((Vector3)(temp * MoveDistance));
                return false;
            }
            NewPos = Target;
            return true;
        }

        /// <summary>
        /// 位置距离2次方（只计算xz）
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static float PosDistance2XZ(Vector3 pos1, Vector3 pos2)
        {
            float num = pos1.x - pos2.x;
            float num2 = pos1.z - pos2.z;
            return ((num * num) + (num2 * num2));
        }

        public static int PosDistance2XZCm(Vector3 pos1, Vector3 pos2)
        {
            return (int)(PosDistance2XZ(pos1, pos2) * 10000);
        }

        public static float PosDistance2XZ(float x1, float z1, float x2, float z2)
        {
            float num = x1 - x2;
            float num2 = z1 - z2;
            return ((num * num) + (num2 * num2));
        }



        /// <summary>
        /// 位置距离（只计算xz）
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static float PosDistanceXZ(Vector3 pos1, Vector3 pos2)
        {
            float num = pos1.x - pos2.x;
            float num2 = pos1.z - pos2.z;
            float dis = Mathf.Sqrt((num * num) + (num2 * num2));
            //pos1.y = 0;
            //pos2.y = 0;
            //Debug.LogError(Vector3.Distance(pos1, pos2) + ":::" + dis);
            return dis;
        }

        /// <summary>
        /// 位置距离厘米（只计算xz）
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static int PosDistanceCmXZ(Vector3 pos1, Vector3 pos2)
        {
            return (int)(PosDistanceXZ(pos1, pos2) * 100);
        }

        public static float PosDistanceXZ(float x1, float z1, float x2, float z2)
        {
            float num = x1 - x2;
            float num2 = z1 - z2;
            return Mathf.Sqrt((num * num) + (num2 * num2));
        }

        public static bool PosNearedXZ(Vector3 pos1, Vector3 pos2)
        {
            float num = pos1.x - pos2.x;
            float num2 = pos1.z - pos2.z;
            return (((num * num) + (num2 * num2)) < 1f);
        }

        public static bool PosIsSameXZ(Vector3 pos1, Vector3 pos2)
        {
            int x1 = (int)(pos1.x * 100);
            int x2 = (int)(pos2.x * 100);
            int z1 = (int)(pos1.z * 100);
            int z2 = (int)(pos2.z * 100);

            return x1 == x2 && z1 == z2;
        }

        /// <summary>
        /// 计算角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int GetAngleXZ(Vector3 from, Vector3 to)
        {
            float x = to.x - from.x;
            float y = to.z - from.z;
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            return Convert.ToInt32(angle);
        }
        /// <summary>
        /// 返回前后左右直线上某点
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector3 GetRandomPosOnLine(Vector3 pos, int length)
        {
            int val = UnityEngine.Random.Range(0, 2);
            bool rdmX = val == 0 ? true : false;
            Vector3 targetPos = pos;

            int posAxis = UnityEngine.Random.Range(-length, length);
            if (rdmX)
            {
                targetPos += new Vector3(posAxis, 0, 0);
            }
            else
            {
                targetPos += new Vector3(0, 0, posAxis);
            }

            //if (BlockManager.IsBlock(targetPos))
            //{
            //    //posTemp = BlockManager.FindNeareatEmptyPoint(pos, posTemp);
            //    targetPos = CheckTargetPos(pos, targetPos, (int)length);
            //    targetPos.y = BlockManager.GetHeight(targetPos);
            //    return targetPos;
            //}
            //else
            //{
            //    targetPos.y = BlockManager.GetHeight(targetPos);
            return targetPos;
            //}
        }
        /// <summary>
        /// 随机位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector3 GetRamdomPos(Vector3 pos, float length)
        {
            float x = UnityEngine.Random.Range(-length, length);
            float z = UnityEngine.Random.Range(-length, length);

            Vector3 posTemp = new Vector3(pos.x + x, 0f, pos.z + z);

            //if (BlockManager.IsBlock(posTemp))
            //{
            //    //posTemp = BlockManager.FindNeareatEmptyPoint(pos, posTemp);
            //    posTemp = CheckTargetPos(pos, posTemp, (int)length);
            //    posTemp.y = BlockManager.GetHeight(posTemp);
            //    return posTemp;
            //}
            //else
            //{
            //    posTemp.y = BlockManager.GetHeight(posTemp);
            return posTemp;
            //}
        }
        /// <summary>
        /// 递归检测目标点是否是障碍，在是障碍点的情况，不断缩短位置
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private static Vector3 CheckTargetPos(Vector3 initPos, Vector3 targetPos, int distance)
        {
            if (distance <= 0)
            {
                return targetPos;
            }

            //if (BlockManager.IsBlock(targetPos))
            //{
            //    distance--;
            //    targetPos = Vector3.MoveTowards(initPos, targetPos, distance);
            //    if (BlockManager.IsBlock(targetPos))
            //    {
            //        return CheckTargetPos(initPos, targetPos, distance);
            //    }
            //}
            return targetPos;
        }
        private static int index = 0;
        /// <summary>
        /// 获取目标反向的随机点 --- 逃跑时使用
        /// 先使用随机点，后面做地图边缘再使用反向
        /// </summary>
        /// <param name="selfPos"></param>
        /// <param name="enemyPos"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public static Vector3 GetReversePos(Vector3 selfPos, Vector3 enemyPos, int dis)
        {
            Vector3 randomPos = Vector3.zero;
            randomPos = GetRamdomPos(selfPos, dis);
            return randomPos;
        }
    }
}