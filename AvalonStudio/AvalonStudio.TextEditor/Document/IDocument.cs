using System;

namespace AvalonStudio.TextEditor.Document
{
#if !NREFACTORY
	/// <summary>
	///     A document representing a source code file for refactoring.
	///     Line and column counting starts at 1.
	///     Offset counting starts at 0.
	/// </summary>
	public interface IDocument : ITextSource, IServiceProvider
	{
#if NREFACTORY
	/// <summary>
	/// Creates an immutable snapshot of this document.
	/// </summary>
		IDocument CreateDocumentSnapshot();
#endif

		/// <summary>
		///     Gets/Sets the text of the whole document..
		/// </summary>
		new string Text { get; set; } // hides ITextSource.Text to add the setter

		/// <summary>
		///     This event is called directly before a change is applied to the document.
		/// </summary>
		/// <remarks>
		///     It is invalid to modify the document within this event handler.
		///     Aborting the change (by throwing an exception) is likely to cause corruption of data structures
		///     that listen to the Changing and Changed events.
		/// </remarks>
		event EventHandler<TextChangeEventArgs> TextChanging;

		/// <summary>
		///     This event is called directly after a change is applied to the document.
		/// </summary>
		/// <remarks>
		///     It is invalid to modify the document within this event handler.
		///     Aborting the event handler (by throwing an exception) is likely to cause corruption of data structures
		///     that listen to the Changing and Changed events.
		/// </remarks>
		event EventHandler<TextChangeEventArgs> TextChanged;

		/// <summary>
		///     This event is called after a group of changes is completed.
		/// </summary>
		/// <seealso cref="EndUndoableAction" />
		event EventHandler ChangeCompleted;

		/// <summary>
		///     Gets the number of lines in the document.
		/// </summary>
		int LineCount { get; }

		/// <summary>
		///     Gets the document line with the specified number.
		/// </summary>
		/// <param name="lineNumber">The number of the line to retrieve. The first line has number 1.</param>
		IDocumentLine GetLineByNumber(int lineNumber);

		/// <summary>
		///     Gets the document line that contains the specified offset.
		/// </summary>
		IDocumentLine GetLineByOffset(int offset);

		/// <summary>
		///     Gets the offset from a text location.
		/// </summary>
		/// <seealso cref="GetLocation" />
		int GetOffset(int line, int column);

		/// <summary>
		///     Gets the offset from a text location.
		/// </summary>
		/// <seealso cref="GetLocation" />
		int GetOffset(TextLocation location);

		/// <summary>
		///     Gets the location from an offset.
		/// </summary>
		/// <seealso cref="GetOffset(TextLocation)" />
		TextLocation GetLocation(int offset);

		/// <summary>
		///     Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <remarks>
		///     Anchors positioned exactly at the insertion offset will move according to their movement type.
		///     For AnchorMovementType.Default, they will move behind the inserted text.
		///     The caret will also move behind the inserted text.
		/// </remarks>
		void Insert(int offset, string text);

		/// <summary>
		///     Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <remarks>
		///     Anchors positioned exactly at the insertion offset will move according to their movement type.
		///     For AnchorMovementType.Default, they will move behind the inserted text.
		///     The caret will also move behind the inserted text.
		/// </remarks>
		void Insert(int offset, ITextSource text);

		/// <summary>
		///     Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <param name="defaultAnchorMovementType">
		///     Anchors positioned exactly at the insertion offset will move according to the anchor's movement type.
		///     For AnchorMovementType.Default, they will move according to the movement type specified by this parameter.
		///     The caret will also move according to the <paramref name="defaultAnchorMovementType" /> parameter.
		/// </param>
		void Insert(int offset, string text, AnchorMovementType defaultAnchorMovementType);

		/// <summary>
		///     Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <param name="defaultAnchorMovementType">
		///     Anchors positioned exactly at the insertion offset will move according to the anchor's movement type.
		///     For AnchorMovementType.Default, they will move according to the movement type specified by this parameter.
		///     The caret will also move according to the <paramref name="defaultAnchorMovementType" /> parameter.
		/// </param>
		void Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType);

		/// <summary>
		///     Removes text.
		/// </summary>
		/// <param name="offset">Starting offset of the text to be removed.</param>
		/// <param name="length">Length of the text to be removed.</param>
		void Remove(int offset, int length);

		/// <summary>
		///     Replaces text.
		/// </summary>
		/// <param name="offset">The starting offset of the text to be replaced.</param>
		/// <param name="length">The length of the text to be replaced.</param>
		/// <param name="newText">The new text.</param>
		void Replace(int offset, int length, string newText);

		/// <summary>
		///     Replaces text.
		/// </summary>
		/// <param name="offset">The starting offset of the text to be replaced.</param>
		/// <param name="length">The length of the text to be replaced.</param>
		/// <param name="newText">The new text.</param>
		void Replace(int offset, int length, ITextSource newText);

		/// <summary>
		///     Make the document combine the following actions into a single
		///     action for undo purposes.
		/// </summary>
		void StartUndoableAction();

		/// <summary>
		///     Ends the undoable action started with <see cref="StartUndoableAction" />.
		/// </summary>
		void EndUndoableAction();

		/// <summary>
		///     Creates an undo group. Dispose the returned value to close the undo group.
		/// </summary>
		/// <returns>An object that closes the undo group when Dispose() is called.</returns>
		IDisposable OpenUndoGroup();

		/// <summary>
		///     Creates a new <see cref="ITextAnchor" /> at the specified offset.
		/// </summary>
		/// <inheritdoc cref="ITextAnchor" select="remarks|example" />
		ITextAnchor CreateAnchor(int offset);

		/// <summary>
		///     Gets the name of the file the document is stored in.
		///     Could also be a non-existent dummy file name or null if no name has been set.
		/// </summary>
		string FileName { get; }

		/// <summary>
		///     Fired when the file name of the document changes.
		/// </summary>
		event EventHandler FileNameChanged;
	}

	/// <summary>
	///     A line inside a <see cref="IDocument" />.
	/// </summary>
	public interface IDocumentLine : ISegment
	{
		/// <summary>
		///     Gets the length of this line, including the line delimiter.
		/// </summary>
		int TotalLength { get; }

		/// <summary>
		///     Gets the length of the line terminator.
		///     Returns 1 or 2; or 0 at the end of the document.
		/// </summary>
		int DelimiterLength { get; }

		/// <summary>
		///     Gets the number of this line.
		///     The first line has the number 1.
		/// </summary>
		int LineNumber { get; }

		/// <summary>
		///     Gets the previous line. Returns null if this is the first line in the document.
		/// </summary>
		IDocumentLine PreviousLine { get; }

		/// <summary>
		///     Gets the next line. Returns null if this is the last line in the document.
		/// </summary>
		IDocumentLine NextLine { get; }

		/// <summary>
		///     Gets whether the line was deleted.
		/// </summary>
		bool IsDeleted { get; }
	}

	/// <summary>
	///     Describes a change of the document text.
	///     This class is thread-safe.
	/// </summary>
	
	public class TextChangeEventArgs : EventArgs
	{
		/// <summary>
		///     Creates a new TextChangeEventArgs object.
		/// </summary>
		public TextChangeEventArgs(int offset, string removedText, string insertedText)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", offset, "offset must not be negative");
			Offset = offset;
			RemovedText = removedText != null ? new StringTextSource(removedText) : StringTextSource.Empty;
			InsertedText = insertedText != null ? new StringTextSource(insertedText) : StringTextSource.Empty;
		}

		/// <summary>
		///     Creates a new TextChangeEventArgs object.
		/// </summary>
		public TextChangeEventArgs(int offset, ITextSource removedText, ITextSource insertedText)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", offset, "offset must not be negative");
			Offset = offset;
			RemovedText = removedText ?? StringTextSource.Empty;
			InsertedText = insertedText ?? StringTextSource.Empty;
		}

		/// <summary>
		///     The offset at which the change occurs.
		/// </summary>
		public int Offset { get; }

		/// <summary>
		///     The text that was removed.
		/// </summary>
		public ITextSource RemovedText { get; }

		/// <summary>
		///     The number of characters removed.
		/// </summary>
		public int RemovalLength
		{
			get { return RemovedText.TextLength; }
		}

		/// <summary>
		///     The text that was inserted.
		/// </summary>
		public ITextSource InsertedText { get; }

		/// <summary>
		///     The number of characters inserted.
		/// </summary>
		public int InsertionLength
		{
			get { return InsertedText.TextLength; }
		}

		/// <summary>
		///     Gets the new offset where the specified offset moves after this document change.
		/// </summary>
		public virtual int GetNewOffset(int offset, AnchorMovementType movementType = AnchorMovementType.Default)
		{
			if (offset >= Offset && offset <= Offset + RemovalLength)
			{
				if (movementType == AnchorMovementType.BeforeInsertion)
					return Offset;
				return Offset + InsertionLength;
			}
			if (offset > Offset)
			{
				return offset + InsertionLength - RemovalLength;
			}
			return offset;
		}

		/// <summary>
		///     Creates TextChangeEventArgs for the reverse change.
		/// </summary>
		public virtual TextChangeEventArgs Invert()
		{
			return new TextChangeEventArgs(Offset, InsertedText, RemovedText);
		}
	}
#endif
}