# Task-05
Data structures. Generics

- Разработать собственный обобщённый тип «бинарное дерево» для хранения результатов тестов, выполненных студентами, и их предоставления в упорядоченном виде.
- Предусмотреть возможность хранения данных любого типа, поддерживающего возможные сравнения.
- Информация о студенте может содержать поля, хранящие имя студента, название теста, дату его прохождения и оценку теста для данного студента.
- Реализовать алгоритм балансировки дерева.
- Обеспечить возможность сериализации и десерилизации дерева в XMLфайл.
- Разработать юнит-тесты для тестирования созданных классов.

Project structure:
- BinaryTree.cs - Class describing the implementation of a binary search tree.
- BinaryTreeNode.cs - The BinaryTreeNode represents a single node in the tree.
- XmlConverter.cs - Class for working with xml files.
- Student.cs - Class describing the student's test result.
- ClassLibraryUnitTests.cs - NUnitTests.
