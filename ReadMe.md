# 飞翔教学资源助手 - 国家中小学智慧教育平台资源解析下载工具

飞翔教学资源助手可以解析下载国家中小学智慧教育平台教材、课程资源地址，支持批量下载，需要.Net Framwork 4.0以上版本支持。

下载地址 [https://github.com/cjhdevact/FlyEduDownloader/releases](https://github.com/cjhdevact/FlyEduDownloader/releases)

本软件完全免费开源，任何人不得用于商业用途，如果你下载本软件是付费后才可下载的，请立刻举报并反馈。

如果有Bug可以反馈到[这里](https://github.com/cjhdevact/FlyEduDownloader/issues)

程序主页 [https://cjhdevact.github.io/otherprojects/FlyEduDownloader/index.html](https://cjhdevact.github.io/otherprojects/FlyEduDownloader/index.html)

使用教程 [https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html](https://cjhdevact.github.io/otherprojects/FlyEduDownloader/Help/index.html)

## 功能说明

本程序支持的功能有：

- [x] 获取智慧教育平台电子教材信息以及下载链接
- [x] 获取智慧教育平台课程资源信息以及下载链接
- [x] 免登录下载智慧教育平台电子教材和课程资源
- [x] 登录下载智慧教育平台电子教材和课程资源
- [x] 自动识别教材标题，自动重命名
- [x] 批量解析和下载智慧教育平台电子教材链接
- [x] 其他一些小工具（如文本处理，批量下载文件（可以使用X-Nd-Auth标头批量下载获取到的教材以及课程资源PDF文件链接）...）
- [x] 100％开源。欢迎提出改进建议。

## 程序截图

![主程序界面](Assets/MainUI.png)


## 开源说明

本软件仅使用了合法的下载技术，通过官方API获取教材链接，本软件自身不存储任何课本资源，课本资源均来自国家的开放平台。

在延伸的代码中（修改和由本仓库代码衍生的代码中）需要说明“基于飞翔教学资源助手（ https://github.com/cjhdevact/FlyEduDownloader ） 开发”。


## 此项目使用的API

### 智慧教育平台教材下载

#### 链接格式：

普通教材：
`https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=（教材contentId）&catalogType=tchMaterial&subCatalog=tchMaterial`

资源包教材：
`https://basic.smartedu.cn/tchMaterial/detail?contentType=thematic_course&contentId=（教材contentId）&catalogType=tchMaterial&subCatalog=tchMateria`

#### 解析接口：

普通教材动态解析，解析里面的PDF文件和标题，但是要带上X-Nd-Auth标头：
`https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/tch_material/details/（教材contentId）.json`

资源包教材：
`https://s-file-1.ykt.cbern.com.cn/zxx/ndrs/special_edu/thematic_course/（教材contentId）/resources/list.json`

#### 示例网页链接

普通教材：

`https://basic.smartedu.cn/tchMaterial/detail?contentType=assets_document&contentId=bdc00134-465d-454b-a541-dcd0cec4d86e&catalogType=tchMaterial&subCatalog=tchMaterial` 

带资源包教材：

`https://basic.smartedu.cn/tchMaterial/detail?contentType=thematic_course&contentId=2afcdb56-6fce-8c99-0bc9-e9dd33b5c51c&catalogType=tchMaterial&subCatalog=tchMaterial`

### 智慧教育平台课程资源下载

#### 链接格式：

教育部资源：

`https://basic.smartedu.cn/syncClassroom/classActivity?activityId=（资源包activityId）&chapterId=&teachingmaterialId=&fromPrepare=0` 

学校提供网课：

`https://basic.smartedu.cn/qualityCourse?courseId=（资源包teachingmaterialId）&chapterId=&teachingmaterialId=&fromPrepare=0&classHourId=lesson_1`

备课：

`https://basic.smartedu.cn/syncClassroom/prepare/detail?lessonId=（资源包lessonId）&chapterId=&teachingmaterialId=&fromPrepare=1&classHourId=lesson_1`

#### 解析接口：

动态解析，解析里面的资源文件和标题，但是要带上X-Nd-Auth标头：

教育部资源：

`https://s-file-1.ykt.cbern.com.cn/zxx/ndrv2/national_lesson/resources/details/（资源包activityId）.json`

学校提供网课：

`https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/resources/（资源包teachingmaterialId）.json`

备课：

`https://s-file-2.ykt.cbern.com.cn/zxx/ndrv2/prepare_lesson/resources/details/（资源包lessonId）.json`

#### 示例网页链接

教育部资源：

`https://basic.smartedu.cn/syncClassroom/classActivity?activityId=f15feef1-b908-44f5-a765-500b9395c313&chapterId=8d6cc118-a169-3ea1-9a45-31cc841ad239&teachingmaterialId=4a4aa279-8dc6-4098-b45f-dd3f7d5a61b2&fromPrepare=0` 

学校提供网课：

`https://basic.smartedu.cn/qualityCourse?courseId=8ae7e48f-842c-12fc-0184-35dacdee016f&chapterId=8ae5c0d4-cfd4-34d1-9757-0295bd0c55ed&teachingmaterialId=4a4aa279-8dc6-4098-b45f-dd3f7d5a61b2&fromPrepare=0&classHourId=lesson_1`

备课：

`https://basic.smartedu.cn/syncClassroom/prepare/detail?lessonId=8aee80a5-6b86-5bc9-016b-87465e6e0290&chapterId=5bb731e1-cdac-3984-a977-3d44c5d2d809&teachingmaterialId=4a4aa279-8dc6-4098-b45f-dd3f7d5a61b2&fromPrepare=1&classHourId=lesson_1`

## 致谢

[AnyTextbookDownloader](https://gitlab.com/xiaoyangtech1/AnyTextbookDownloader) - 参考了教材下载思路。

------------


本程序基于 `GPL-3.0` 授权。
