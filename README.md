# UGUI-InfiniteScrollList
## 进度: 40%
Unity UGUI 高效的无限循环复用列表的实现       
        
1. 支持CenterClick, CenterChild, SpringPanel, MoveTo
2. 分帧加载, 无需初始化的时候就全部构建cell,而是用时创建
3. 完整的 热更层与框架层 交互示例
4. 建此repo的目的是实现：开心消消乐的排行榜名次动态变化的效果【还未完整实现】

<p align="center" >
<img src="https://github.com/kaclok/Unity-UGUI-InfiniteScrolList/blob/master/Gifs/gif.gif" alt="InfiniteScrolList" title="InfiniteScrolList view">
</p>

<p align="center" >
<img src="https://github.com/kaclok/Unity-UGUI-InfiniteScrolList/blob/master/Gifs/rank.gif" alt="Rank" title="Rank view">
</p>

实现过程中遇到的问题：
1. 短时间内快速MoveTo超过Line*(Width/Height)的时候,会出现Content位置正确，但是Content内Group位置不正确,原因就是默认都是按照+- 1 * Line*(Width/Height)处理的，而应该是+= n * Line*(Width/Height)

参考了：
1. NGUI的 CenterOnChild, CenterOnClick, SpringPanel
2. https://github.com/HengyuanLee/UGUIScrollGrid.git
3. https://github.com/akbiggs/UnityTimer.git
4. https://github.com/kaclok/unity-ui-extensions.git
5. https://github.com/kaclok/ugui-super-scrollview-example.git
6. https://blog.csdn.net/qq_30259857/article/details/80275920
7. https://github.com/rlafydid/UGUICircularScrollView
8. https://blog.csdn.net/qq_30259857/article/details/79562652
9. https://github.com/kaclok/UnityTableView
