
using System.ComponentModel;

namespace VersionInfo
{
    /// <summary>
    /// �汾����ö��
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// ��ʽ�����汾 - ����������������
        /// </summary>
        [Description("��ʽ�����汾")]
        Release,

        /// <summary>
        /// �����汾 - ���ܿ������ڲ�����ʹ��
        /// </summary>
        [Description("�����汾")]
        Dev,

        /// <summary>
        /// ���޸��汾 - ���������޸�
        /// </summary>
        [Description("���޸��汾")]
        Hotfix,

        /// <summary>
        /// ���԰汾 - ����ǰ�ĺ�ѡ�汾
        /// </summary>
        [Description("���԰汾")]
        Beta
    }


}
