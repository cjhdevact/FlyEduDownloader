# SmartEduDownloader - 国家中小学智慧教育平台教材解析下载工具

SmartEduDownloader 可以解析下载国家中小学智慧教育平台教材的文档地址，支持批量下载，需要.Net Framwork 4.0以上版本支持。

MD5 Hash
SmartEduDownloader-1.0.24081.0-x86.7z D0E175C2271E5AAD46225F7C25008FD6
SmartEduDownloader-1.0.24081.0-x64.7z 5F9A82D85649D52D45D1719561A70379

下载地址 [https://github.com/cjhdevact/SmartEduDownloader/releases](https://github.com/cjhdevact/SmartEduDownloader/releases)

本软件完全免费开源，任何人不得用于商业用途，如果你下载本软件是付费后才可下载的，请立刻举报并反馈。

如果有Bug可以反馈到[这里](https://github.com/cjhdevact/SmartEduDownloader/issues)

程序主页 [https://cjhdevact.github.io/otherprojects/SmartEduDownloader/index.html](https://cjhdevact.github.io/otherprojects/SmartEduDownloader/index.html)

使用教程 [https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html](https://cjhdevact.github.io/otherprojects/SmartEduDownloader/Help/index.html)


## 程序截图

![主程序界面](Assets/MainUI.png)


## 开源说明

本软件仅使用了合法的下载技术，本软件自身不存储任何课本资源，课本资源均来自国家的开放平台。

在延伸的代码中（修改和由本仓库代码衍生的代码中）需要说明“基于 SmartEduDownloader（ https://github.com/cjhdevact/SmartEduDownloader ） 开发”。


## 此项目使用的API

### 智慧教育平台

#### 链接格式：

`https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=一段字符&catalogType=tchMaterial&subCatalog=tchMaterial`

#### 解析接口：

动态解析，解析里面的PDF文件和标题，但是要带上X-Nd-Auth标头：
`https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/{content_id}.json`

#### 示例课本网页链接：小学道法一上

`https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=bdc00134-465d-454b-a541-dcd0cec4d86e&catalogType=tchMaterial&subCatalog=tchMaterial` 

## 致谢

[AnyTextbookDownloader](https://gitlab.com/xiaoyangtech1/AnyTextbookDownloader) - 提供了教材下载思路

------------


本程序基于 `GPL-3.0` 授权。
